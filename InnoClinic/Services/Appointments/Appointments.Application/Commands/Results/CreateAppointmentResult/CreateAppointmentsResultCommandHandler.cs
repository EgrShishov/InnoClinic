public class CreateAppointmentsResultCommandHandler(
    IMediator mediator,
    IUnitOfWork unitOfWork,
    IPDFDocumentGenerator documentGenerator,
    IProfilesHttpClient profilesHttpClient,
    IFilesHttpClient filesHttpClient) : IRequestHandler<CreateAppointmentsResultCommand, ErrorOr<Results>>
{
    public async Task<ErrorOr<Results>> Handle(CreateAppointmentsResultCommand request, CancellationToken cancellationToken)
    {
        var service = await unitOfWork.ServiceRepository.GetServiceByIdAsync(request.ServiceId);
        
        if (service is null)
        {
            return Errors.Service.NotFound(request.ServiceId);
        }

        var appointment = await unitOfWork.AppointmentsRepository.GetAppointmentByIdAsync(request.AppointmentId);

        if (appointment is null)
        {
            return Errors.Appointments.NotFound;
        }

/*        var result = await unitOfWork.ResultsRepository.ListAsync(r => r.AppointmentId == request.AppointmentId);

        if (result is not null)
        {
            return Errors.Results.AlreadyExists;
        }*/

        var patientProfileResponse = await profilesHttpClient.GetPatientAsync(request.PatientId);

        if (patientProfileResponse.IsError)
        {
            return patientProfileResponse.FirstError;
        }

        var patientProfile = patientProfileResponse.Value;

        var doctorsProfileResponse = await profilesHttpClient.GetDoctorAsync(request.DoctorId);

        if (doctorsProfileResponse.IsError)
        {
            return doctorsProfileResponse.FirstError;
        }

        var doctorProfile = doctorsProfileResponse.Value;

        var appointmetsResult = new Results
        {
            Complaints = request.Complaints,
            Conclusion = request.Conclusion,
            Date = appointment.Date,
            Recommendations = request.Recommendations,
            AppointmentId = appointment.Id,
            Appointment = appointment
        };

        var newResults = await unitOfWork.ResultsRepository.AddResultsAsync(appointmetsResult, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        string doctorsName = $"{doctorProfile.LastName} {doctorProfile.FirstName} {doctorProfile.MiddleName}";
        string patientsName = $"{patientProfile.LastName} {patientProfile.FirstName} {patientProfile.MiddleName}";

        var pdfRequest = new GeneratePDFResultsRequest
        {
            Date = newResults.Date.Date,
            DoctorName = doctorsName,
            PatientName = patientsName,
            DateOfBirth = patientProfile.DateOfBirth,
            Complaints = newResults.Complaints,
            Conclusion = newResults.Conclusion,
            Recommendations = newResults.Recommendations,
            ServiceName = service.ServiceName,
            Specialization = doctorProfile.SpecializationName
        };

        var resultsFile = documentGenerator.GenerateAppointmentResults(pdfRequest);
            
        if (resultsFile is null)
        {
            return Errors.Results.CannotGeneratePDF;
        }

        var uploadingResponse = await filesHttpClient.UploadDocumentAsync(resultsFile, newResults.Id);

        if (uploadingResponse.IsError)
        {
            return uploadingResponse.FirstError;
        }

        await mediator.Send(new NotifyAppointmentsResultCreatedCommand(resultsFile, newResults.Date, patientProfile.UserId));

        return newResults;
    }
}
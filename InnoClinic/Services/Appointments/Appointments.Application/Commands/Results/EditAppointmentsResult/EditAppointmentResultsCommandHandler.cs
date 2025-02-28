public class EditAppointmentResultsCommandHandler(
    IMediator mediator,
    IUnitOfWork unitOfWork,
    IPDFDocumentGenerator documentGenerator,
    IProfilesHttpClient profilesHttpClient,
    IFilesHttpClient filesHttpClient) : IRequestHandler<EditAppointmentsResultCommand, ErrorOr<Results>>
{
    public async Task<ErrorOr<Results>> Handle(EditAppointmentsResultCommand request, CancellationToken cancellationToken)
    {
        var results = await unitOfWork.ResultsRepository.GetResultsByIdAsync(request.ResultsId);
        
        if (results is null)
        {
            return Errors.Results.NotFound;
        }

        var appointment = await unitOfWork.AppointmentsRepository.GetAppointmentByIdAsync(results.AppointmentId);

        if (appointment is null)
        {
            return Errors.Appointments.NotFound;
        }

        results.Complaints = request.Complaints;
        results.Conclusion = request.Conclusion;
        results.Recommendations = request.Recommendations;

        await unitOfWork.ResultsRepository.UpdateResultsAsync(results, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var patientProfileResponse = await profilesHttpClient.GetPatientAsync(appointment.PatientId);

        if (patientProfileResponse.IsError)
        {
            return patientProfileResponse.FirstError;
        }

        var patientProfile = patientProfileResponse.Value;

        var doctorsProfileResponse = await profilesHttpClient.GetDoctorAsync(appointment.DoctorId);

        if (doctorsProfileResponse.IsError)
        {
            return doctorsProfileResponse.FirstError;
        }

        var service = await unitOfWork.ServiceRepository.GetServiceByIdAsync(appointment.ServiceId);

        if (service is null)
        {
            return Errors.Service.NotFound(appointment.ServiceId);
        }

        var doctorProfile = doctorsProfileResponse.Value;

        string doctorsName = $"{doctorProfile.LastName} {doctorProfile.FirstName} {doctorProfile.MiddleName}";
        string patientsName = $"{patientProfile.LastName} {patientProfile.FirstName} {patientProfile.MiddleName}";

        var regeneratePdfRequest = new GeneratePDFResultsRequest
        {
            Date = results.Date.Date,
            DoctorName = doctorsName,
            PatientName = patientsName,
            DateOfBirth = patientProfile.DateOfBirth,
            Complaints = results.Complaints,
            Conclusion = results.Conclusion,
            Recommendations = results.Recommendations,
            ServiceName = service.ServiceName,
            Specialization = doctorProfile.SpecializationName
        };

        var resultsFile = documentGenerator.GenerateAppointmentResults(regeneratePdfRequest);

        var updatingPdfResponse = await filesHttpClient.UploadDocumentAsync(resultsFile, request.ResultsId);

        if (updatingPdfResponse.IsError)
        {
            return updatingPdfResponse.FirstError;
        }

        await mediator.Send(new NotifyAppointmentsResultChangedCommand(request.ResultsId));

        return results;
    }
}

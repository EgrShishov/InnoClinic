public class ViewAppointmentResultsQueryHandler(
    IUnitOfWork unitOfWork,
    IProfilesHttpClient profilesHttpClient,
    ICacheService cacheService)
    : IRequestHandler<ViewAppointmentResultQuery, ErrorOr<ResultResponse>>
{
    public async Task<ErrorOr<ResultResponse>> Handle(ViewAppointmentResultQuery request, CancellationToken cancellationToken)
    {
        ResultResponse? result = await cacheService.
            GetAsync<ResultResponse>($"results {request.ResultsId}", cancellationToken);

        if (result is not null)
        {
            return result;
        }

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

        var patientProfileResponse = await profilesHttpClient.GetPatientAsync(appointment.PatientId);

        if (patientProfileResponse.IsError)
        {
            return patientProfileResponse.FirstError;
        }

        var patientProfile = patientProfileResponse.Value;

        string patientFullName = $"{patientProfile.LastName} {patientProfile.FirstName} {patientProfile.MiddleName}";

        var doctorProfileResponse = await profilesHttpClient.GetDoctorAsync(appointment.DoctorId);

        if (doctorProfileResponse.IsError)
        {
            return doctorProfileResponse.FirstError;
        }

        var doctorProfile = doctorProfileResponse.Value;

        string doctorFullName = $"{doctorProfile.LastName} {doctorProfile.FirstName} {doctorProfile.MiddleName}";

        var serviceInfo = await unitOfWork.ServiceRepository.GetServiceByIdAsync(appointment.ServiceId);

        if (serviceInfo is null)
        {
            return Errors.Service.NotFound(appointment.ServiceId);
        }

        var resultResponse = new ResultResponse
        {
            AppointmentDate = results.Date,
            PatientsFullName = patientFullName,
            PatientDateOfBirth = patientProfile.DateOfBirth,
            DoctorsFullName = doctorFullName,
            DoctorsSpecialization = doctorProfile.SpecializationName,
            ServiceName = serviceInfo.ServiceName,
            Complaints = results.Complaints,
            Conclusion = results.Conclusion,
            Reccomendations = results.Recommendations
        };

        await cacheService.SetAsync($"results {request.ResultsId}", resultResponse, cancellationToken);

        return resultResponse;
    }
}

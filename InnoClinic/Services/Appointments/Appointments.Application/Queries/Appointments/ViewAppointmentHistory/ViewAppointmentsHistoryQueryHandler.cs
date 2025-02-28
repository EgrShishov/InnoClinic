public class ViewAppointmentsHistoryQueryHandler(
    IUnitOfWork unitOfWork, 
    IProfilesHttpClient profilesHttpClient,
    ICacheService cacheService)
    : IRequestHandler<ViewAppointmentsHistoryQuery, ErrorOr<List<AppointmentHistoryResponse>>>
{
    public async Task<ErrorOr<List<AppointmentHistoryResponse>>> Handle(ViewAppointmentsHistoryQuery request, CancellationToken cancellationToken)
    {
        List<AppointmentHistoryResponse>? appointmentsHistory = await cacheService.
            GetAsync<List<AppointmentHistoryResponse>>($"appointments-history {request.PatientId}");

        if (appointmentsHistory is not null)
        {
            return appointmentsHistory;
        }

        var appointments = await unitOfWork.AppointmentsRepository.ListAsync(a => a.PatientId == request.PatientId);

        if (appointments is null || !appointments.Any())
        {
            return Errors.Appointments.NotFound;
        }

        var historyResponses = new List<AppointmentHistoryResponse>();

        foreach (Appointment appointment in appointments)
        {
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

            historyResponses.Add(new AppointmentHistoryResponse
            {
                AppointmentDate = appointment.Date,
                AppointmentTime = appointment.Time,
                DoctorFullName = doctorFullName,
                ServiceName = serviceInfo.ServiceName,
                LinkToMedicalResults = appointment.IsApproved ? $"appointments/{appointment.Id}/results" : null
            });
        }

        if (!historyResponses.Any())
        {
            return Errors.Appointments.EmptyHistory;
        }

        appointmentsHistory = historyResponses;

        await cacheService.SetAsync($"appointment-history {request.PatientId}", appointmentsHistory, cancellationToken);

        return appointmentsHistory;
    }
}

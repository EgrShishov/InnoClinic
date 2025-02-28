public class ViewAppointmentScheduleQueryHandler(
    IUnitOfWork unitOfWork, 
    IProfilesHttpClient profilesHttlClient,
    ICacheService cacheService)
    : IRequestHandler<ViewAppointmentScheduleQuery, ErrorOr<List<AppointmentsScheduleResponse>>>
{
    public async Task<ErrorOr<List<AppointmentsScheduleResponse>>> Handle(ViewAppointmentScheduleQuery request, CancellationToken cancellationToken)
    {
        List<AppointmentsScheduleResponse>? schedule = await cacheService.
            GetAsync<List<AppointmentsScheduleResponse>>($"schedule {request.AppointmentDate} {request.DoctorId}", cancellationToken);

        if (schedule is not null)
        {
            return schedule;
        }

        var doctorProfileResponse = await profilesHttlClient.GetDoctorAsync(request.DoctorId);

        if (doctorProfileResponse.IsError)
        {
            return doctorProfileResponse.FirstError;
        }

        var appointments = await unitOfWork.AppointmentsRepository.
            ListAsync(a => a.DoctorId == request.DoctorId && a.Date.Date == request.AppointmentDate.Date);

        if (appointments is null || !appointments.Any())
        {
            return Errors.Appointments.EmptyList;
        }

        List<AppointmentsScheduleResponse?> appointmentsSchedule = new();

        foreach (var appointment in appointments)
        {
            var patientProfileResponse = await profilesHttlClient.GetPatientAsync(appointment.PatientId);

            if (patientProfileResponse.IsError)
            {
                return patientProfileResponse.FirstError;
            }

            var patientProfile = patientProfileResponse.Value;

            var service = await unitOfWork.ServiceRepository.GetServiceByIdAsync(appointment.ServiceId);

            if (service is null)
            {
                return Errors.Service.NotFound(appointment.ServiceId);
            }

            appointmentsSchedule.Add(new AppointmentsScheduleResponse
            {
                Time = appointment.Time,
                PatientFullName = $"{patientProfile.LastName} {patientProfile.FirstName} {patientProfile.MiddleName}",
                PatientProfileLink = $"profileservice.api/patients/{appointment.PatientId}",
                ServiceName = service.ServiceName,
                ApprovalStatus = appointment.IsApproved ? "Approved" : "Not approved",
                MedicalResultsLink = appointment.IsApproved ? $"appointments/{appointment.Id}/results" : null
            });
        }

        if (!appointmentsSchedule.Any() || appointmentsSchedule is null)
        {
            return Errors.Appointments.EmptySchedule;
        }

        schedule = appointmentsSchedule.OrderBy(a => a.Time).ToList();

        await cacheService.SetAsync($"schedule {request.AppointmentDate} {request.DoctorId}", schedule, cancellationToken);

        return schedule;
    }
}

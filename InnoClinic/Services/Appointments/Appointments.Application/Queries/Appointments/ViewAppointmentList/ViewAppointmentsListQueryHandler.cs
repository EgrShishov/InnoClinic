public class ViewAppointmentsListQueryHandler(
    IUnitOfWork unitOfWork, 
    IProfilesHttpClient profilesHttpClient,
    ICacheService cacheService) 
    : IRequestHandler<ViewAppointmentsListQuery, ErrorOr<List<AppointmentListResponse>>>
{
    public async Task<ErrorOr<List<AppointmentListResponse>>> Handle(ViewAppointmentsListQuery request, CancellationToken cancellationToken)
    {
        if (request.ServiceId.HasValue)
        {
            var service = await unitOfWork.ServiceRepository.GetServiceByIdAsync(request.ServiceId.Value);

            if (service is null)
            {
                return Errors.Service.NotFound(request.ServiceId.Value);
            }
        }

        if (request.DoctorId.HasValue)
        {
            var doctorProfileResponse = await profilesHttpClient.GetDoctorAsync(request.DoctorId.Value);

            if (doctorProfileResponse.IsError)
            {
                return doctorProfileResponse.FirstError;
            }
        }

/*        List<AppointmentListResponse>? appointmentsList = await cacheService.
            GetAsync<List<AppointmentListResponse>>("appointments", cancellationToken);*/

        var appointments = await unitOfWork.AppointmentsRepository.GetAllAsync(cancellationToken)
            .ContinueWith(task =>
            {
                var appointments = task.Result;
                var query = appointments.AsQueryable();

                query = query.Where(a => a.IsApproved == request.AppointmentStatus);

                if (request.AppointmentDate.HasValue)
                {
                    query = query.Where(a => a.Date.Date == request.AppointmentDate.Value.Date);
                }

                if (request.DoctorId.HasValue)
                {
                    query = query.Where(a => a.DoctorId == request.DoctorId);
                }

                if (request.ServiceId.HasValue)
                {
                    query = query.Where(a => a.ServiceId == request.ServiceId);
                }

                return query.ToList();
            });

        var appointmentListResponses = new List<AppointmentListResponse>();

        foreach (var appointment in appointments)
        {
            var doctorProfileResponse = await profilesHttpClient.GetDoctorAsync(appointment.DoctorId);

            if (doctorProfileResponse.IsError)
            {
                return doctorProfileResponse.FirstError;
            }

            var doctorProfile = doctorProfileResponse.Value;

            var patientProfileResponse = await profilesHttpClient.GetPatientAsync(appointment.PatientId);

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

            string doctorFullName = $"{doctorProfile.LastName} {doctorProfile.FirstName} {doctorProfile.MiddleName}";
            string patientFullName = $"{patientProfile.LastName} {patientProfile.FirstName} {patientProfile.MiddleName}";

            appointmentListResponses.Add(new AppointmentListResponse
            {
                AppointmentTime = appointment.Time,
                DoctorFullName = doctorFullName,
                PatientFullName = patientFullName,
                PatientPhoneNumber = patientProfile.PhoneNumber,
                ServiceName = service.ServiceName
            });
        }

        if (!appointmentListResponses.Any())
        {
            return Errors.Appointments.EmptyList;
        }

        var appointmentsList = appointmentListResponses;

        //await cacheService.SetAsync("appointments", appointmentsList, cancellationToken);

        return appointmentsList;
    }
}

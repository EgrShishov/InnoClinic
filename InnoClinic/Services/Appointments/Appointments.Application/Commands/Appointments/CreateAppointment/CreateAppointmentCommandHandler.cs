public class CreateAppointmentCommandHandler(
    IUnitOfWork unitOfWork,
    IProfilesHttpClient profilesHttpClient) : IRequestHandler<CreateAppointmentCommand, ErrorOr<Appointment>>
{
    public async Task<ErrorOr<Appointment>> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var simmilarAppointments = await unitOfWork
            .AppointmentsRepository
            .ListAsync(a => a.PatientId == request.PatientId &&
                        a.ServiceId == request.ServiceId &&
                        a.DoctorId == request.DoctorId &&
                        a.Date == request.Date &&
                        a.Time == request.Time);
        
        if (simmilarAppointments.Any())
        {
            return Errors.Appointments.AlreadyCreated;
        }

        var doctorInfoResponse = await profilesHttpClient.GetDoctorAsync(request.DoctorId);

        if (doctorInfoResponse.IsError)
        {
            return doctorInfoResponse.FirstError;
        }

        var doctroProfile = doctorInfoResponse.Value;

        var patientInfoResponse = await profilesHttpClient.GetPatientAsync(request.PatientId);

        if (patientInfoResponse.IsError)
        {
            return patientInfoResponse.FirstError;
        }

        var service = await unitOfWork.ServiceRepository.GetServiceByIdAsync(request.ServiceId);

        if (service is null)
        {
            return Errors.Service.NotFound(request.ServiceId);
        }

        var appointment = new Appointment
        {
            PatientId = request.PatientId,
            IsApproved = false,
            Date = request.Date,
            Time = request.Time,
            DoctorId = request.DoctorId,
            ServiceId = request.ServiceId
        };

        var newAppointment = await unitOfWork.AppointmentsRepository.AddAppointmentAsync(appointment, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return newAppointment;
    }
}

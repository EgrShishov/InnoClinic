public class RescheduleAppointmentCommandHandler(
    IUnitOfWork unitOfWork,
    ICacheService cacheService) : IRequestHandler<RescheduleAppointmentCommand, ErrorOr<Appointment>>
{
    public async Task<ErrorOr<Appointment>> Handle(RescheduleAppointmentCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var appointment = await unitOfWork.AppointmentsRepository.GetAppointmentByIdAsync(request.AppointmentId);
            
            if (appointment is null)
            {
                return Errors.Appointments.NotFound;
            }

            if (appointment.IsApproved)
            {
                return Errors.Appointments.RescheduleIsImpossibleBecauseIsApproved;
            }

            appointment.Date = request.Date;
            appointment.Time = request.Time;
            appointment.DoctorId = request.DoctorId;

            await unitOfWork.AppointmentsRepository.UpdateAppointmentAsync(appointment, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            List<AppointmentListResponse>? appointmentsList = await cacheService
                .GetAsync<List<AppointmentListResponse>>("appointments", cancellationToken);

            if (appointmentsList is not null)
            {
                await cacheService.RemoveAsync("appointments", cancellationToken);
            }

            return appointment;
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}

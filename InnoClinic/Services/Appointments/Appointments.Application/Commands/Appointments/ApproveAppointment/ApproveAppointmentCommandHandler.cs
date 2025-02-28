public class ApproveAppointmentCommandHandler(
    IUnitOfWork unitOfWork,
    ICacheService cacheService) : IRequestHandler<ApproveAppointmentCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(ApproveAppointmentCommand request, CancellationToken cancellationToken)
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
                return Errors.Appointments.AlreadyApproved;
            }

            appointment.IsApproved = true;

            await unitOfWork.AppointmentsRepository.UpdateAppointmentAsync(appointment, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            List<AppointmentListResponse>? appointmentsList = await cacheService
                .GetAsync<List<AppointmentListResponse>>("appointments", cancellationToken);

            if (appointmentsList is not null)
            {
                await cacheService.RemoveAsync("appointments", cancellationToken);
            }

            List<AppointmentHistoryResponse>? appointmentsHistory = await cacheService.
                GetAsync<List<AppointmentHistoryResponse>>($"appointments-history {appointment.PatientId}");

            if (appointmentsHistory is not null)
            {
                await cacheService.RemoveAsync($"appointments-history {appointment.PatientId}");
            }

            return Unit.Value;
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}

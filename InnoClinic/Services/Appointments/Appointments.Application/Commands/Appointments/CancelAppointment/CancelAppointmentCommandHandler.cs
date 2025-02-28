public class CancelAppointmentCommandHandler(
    IUnitOfWork unitOfWork,
    ICacheService cacheService) : IRequestHandler<CancelAppointmentCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var appointment = await unitOfWork.AppointmentsRepository.GetAppointmentByIdAsync(request.AppointmentId);

            if (appointment is null)
            {
                return Errors.Appointments.NotFound;
            }

            await unitOfWork.AppointmentsRepository.DeleteAppointmentAsync(request.AppointmentId, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            List<AppointmentListResponse>? appointmentsList = await cacheService
                .GetAsync<List<AppointmentListResponse>>("appointments", cancellationToken);

            if (appointmentsList is not null)
            {
                await cacheService.RemoveAsync("appointments", cancellationToken);
            }
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }

        return Unit.Value;
    }
}
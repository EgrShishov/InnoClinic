public sealed class DeleteServiceCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteServiceCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await unitOfWork.Services.GetServiceByIdAsync(request.ServiceId);

        if (service is null)
        {
            return Errors.Service.NotFound;
        }

        await unitOfWork.Services.DeleteService(request.ServiceId, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
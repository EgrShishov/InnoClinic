using InnoClinic.Contracts.ServiceStatusChangedEvent;

public class ChangeServiceStatusCommandHandler(IUnitOfWork unitOfWork, IEventBus eventBus) 
    : IRequestHandler<ChangeServiceStatusCommand, ErrorOr<ServiceInfoResponse>>
{
    public async Task<ErrorOr<ServiceInfoResponse>> Handle(ChangeServiceStatusCommand request, CancellationToken cancellationToken)
    {
        var service = await unitOfWork.Services.GetServiceByIdAsync(request.Id);
        
        if (service is null)
        {
            return Errors.Service.NotFound;
        }

        service.IsActive = request.Status;

        await unitOfWork.Services.UpdateServiceAsync(service);

        await unitOfWork.SaveChangesAsync();

        await eventBus.PublishAsync(
            new ServiceStatusChangedEvent
            {
                Id = service.Id,
                ServiceName = service.ServiceName,
                IsActive = service.IsActive
            },
            cancellationToken);

        return new ServiceInfoResponse
        {
            Id = service.Id,
            ServiceCategoryName = service.ServiceCategory.ToString(),
            ServiceName = service.ServiceName,
            ServicePrice = service.ServicePrice,
            IsActive = service.IsActive
        };
    }
}

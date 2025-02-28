using InnoClinic.Contracts.ServiceCreatedEvent;

public class CreateServiceCommandHandler(IUnitOfWork unitOfWork, IEventBus eventBus) 
    : IRequestHandler<CreateServiceCommand, ErrorOr<ServiceInfoResponse>>
{
    public async Task<ErrorOr<ServiceInfoResponse>> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = new Service
        {
            ServiceName = request.ServiceName,
            ServicePrice = request.ServicePrice,
            ServiceCategory = request.ServiceCategory,
            IsActive = request.IsActive,
            SpecializationId = request.SpecializationId
        };

        var newService = await unitOfWork.Services.AddServiceAsync(service, cancellationToken);
        
        await unitOfWork.SaveChangesAsync();

        await eventBus.PublishAsync(new ServiceCreatedEvent
            {
                Id = newService.Id,
                ServiceCategory = newService.ServiceCategory,
                ServiceName = newService.ServiceName,
                IsActive = newService.IsActive,
            }, 
            cancellationToken);

        return new ServiceInfoResponse
        {
            Id = newService.Id,
            ServiceCategoryName = newService.ServiceCategory.ToString(),
            ServiceName = newService.ServiceName,
            ServicePrice = newService.ServicePrice,
            IsActive = newService.IsActive
        };
    }
}

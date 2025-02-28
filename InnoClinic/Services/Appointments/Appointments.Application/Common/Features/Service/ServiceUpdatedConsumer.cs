using InnoClinic.Contracts.ServiceUpdatedEvent;

public sealed class ServiceUpdatedConsumer : IConsumer<ServiceUpdatedEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public ServiceUpdatedConsumer(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<ServiceUpdatedEvent> context)
    {
        var message = context.Message;

        var service = await _unitOfWork.ServiceRepository.GetServiceByIdAsync(message.Id);
        
        if (service is null)
        {
            return;
        }
    
        service.ServiceName = message.ServiceName;
        service.ServiceCategory = message.ServiceCategory;
        service.IsActive = message.IsActive;

        await _unitOfWork.ServiceRepository.UpdateServiceAsync(service);

        await _unitOfWork.SaveChangesAsync();
    }
}

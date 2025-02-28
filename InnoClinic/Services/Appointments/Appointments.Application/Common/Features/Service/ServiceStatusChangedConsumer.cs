using InnoClinic.Contracts.ServiceStatusChangedEvent;

public sealed class ServiceStatusChangedConsumer : IConsumer<ServiceStatusChangedEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public ServiceStatusChangedConsumer(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<ServiceStatusChangedEvent> context)
    {
        var message = context.Message;

        var service = await _unitOfWork.ServiceRepository.GetServiceByIdAsync(message.Id);
        
        if (service is null)
        {
            return;
        }

        service.IsActive = message.IsActive;
       
        await _unitOfWork.ServiceRepository.UpdateServiceAsync(service);
 
        await _unitOfWork.SaveChangesAsync();
    }
}

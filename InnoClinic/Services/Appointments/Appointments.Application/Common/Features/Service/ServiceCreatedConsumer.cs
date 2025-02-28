using InnoClinic.Contracts.ServiceCreatedEvent;

public sealed class ServiceCreatedConsumer : IConsumer<ServiceCreatedEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public ServiceCreatedConsumer(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<ServiceCreatedEvent> context)
    {
        var message = context.Message;

        var service = new Service
        {
            Id = message.Id,
            IsActive = message.IsActive,
            ServiceCategory = message.ServiceCategory,
            ServiceName = message.ServiceName,
        };

        await _unitOfWork.ServiceRepository.AddServiceAsync(service);

        await _unitOfWork.SaveChangesAsync();
    }
}

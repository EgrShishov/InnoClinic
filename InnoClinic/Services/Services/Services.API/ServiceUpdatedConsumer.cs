using InnoClinic.Contracts.ServiceUpdatedEvent;

public sealed class ServiceUpdatedConsumer : IConsumer<ServiceUpdatedEvent>
{
    public Task Consume(ConsumeContext<ServiceUpdatedEvent> context)
    {
        throw new NotImplementedException();
    }
}


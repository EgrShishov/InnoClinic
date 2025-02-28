using InnoClinic.Contracts.ServiceDeletedEvent;

public class ServiceDeletedConsumer : IConsumer<ServiceDeletedEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    public ServiceDeletedConsumer(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task Consume(ConsumeContext<ServiceDeletedEvent> context)
    {
        var message = context.Message;

        await _unitOfWork.ServiceRepository.DeleteServiceAsync(message.Id);

        await _unitOfWork.SaveChangesAsync();
    }
}
using InnoClinic.Contracts.OfficeDeletedEvent;

public class OfficeDeletedConsumer : IConsumer<OfficeDeletedEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    public OfficeDeletedConsumer(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<OfficeDeletedEvent> context)
    {
        var message = context.Message;
        
        await _unitOfWork.OfficeRepository.DeleteOfficeAsync(message.Id);
        
        await _unitOfWork.CompleteAsync();
    }
}

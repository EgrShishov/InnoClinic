using InnoClinic.Contracts.OfficeStatusChangedEvent;

public class OfficeStatusChangedConsumer : IConsumer<OfficeStatusChangedEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    public OfficeStatusChangedConsumer(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<OfficeStatusChangedEvent> context)
    {
        var message = context.Message;

        var office = await _unitOfWork.OfficeRepository.GetOfficeByIdAsync(message.Id);
        
        if (office is null)
        {
            return;
        }

        office.IsActive = message.IsActive;

        await _unitOfWork.OfficeRepository.UpdateOfficeAsync(office);

        await _unitOfWork.CompleteAsync();
    }
}

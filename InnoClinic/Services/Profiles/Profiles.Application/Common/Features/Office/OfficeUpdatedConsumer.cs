using InnoClinic.Contracts.OfficeChangedEvent;

public sealed class OfficeUpdatedConsumer : IConsumer<OfficeChangedEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    public OfficeUpdatedConsumer(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<OfficeChangedEvent> context)
    {
        var message = context.Message;
        
        var office = await _unitOfWork.OfficeRepository.GetOfficeByIdAsync(message.Id);
        
        if (office is null)
        {
            return;
        }

        office.Id = message.Id;
        office.City = message.City;
        office.HouseNumber = message.HouseNumber;
        office.OfficeNumber = message.OfficeNumber;
        office.PhotoId = message.PhotoId;
        office.RegistryPhoneNumber = message.RegistryPhoneNumber;
        office.Street = message.Street;

        await _unitOfWork.OfficeRepository.UpdateOfficeAsync(office);

        await _unitOfWork.CompleteAsync();
    }
}
using InnoClinic.Contracts.OfficeCreatedEvent;

public class CreateOfficeCommandHandler(
    IUnitOfWork unitOfWork, 
    IFilesHttpClient filesHttpClient, 
    IEventBus eventBus) 
    : IRequestHandler<CreateOfficeCommand, ErrorOr<OfficeResponse>>
{
    public async Task<ErrorOr<OfficeResponse>> Handle(CreateOfficeCommand request, CancellationToken cancellationToken)
    {
        var photoUrl = await filesHttpClient.UploadPhoto(request.Photo);
        
        if (photoUrl.IsError)
        {
            return Errors.FilesApi.UploadingError;
        }

        var office = new Office
        {
            City = request.City,
            HouseNumber = request.HouseNumber,
            IsActive = request.IsActive,
            OfficeNumber = request.OfficeNumber,
            PhotoId = photoUrl.Value,
            RegistryPhoneNumber = request.RegistryPhoneNumber,
            Street = request.Street
        };

        await unitOfWork.OfficeRepository.AddOfficeAsync(office, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await eventBus.PublishAsync(new OfficeCreatedEvent
        {
            Id = office.Id.ToString(),
            City = office.City,
            HouseNumber = office.HouseNumber,
            OfficeNumber = office.OfficeNumber,
            PhotoId = office.PhotoId,
            RegistryPhoneNumber = office.RegistryPhoneNumber,
            Street = office.Street
        });

        return new OfficeResponse 
        {
            Id = office.Id.ToString(),
            Address = office.Address,
            PhotoUrl = office.PhotoId,
            IsActive = office.IsActive,
            RegistryPhoneNumber = office.RegistryPhoneNumber
        };
    }
}

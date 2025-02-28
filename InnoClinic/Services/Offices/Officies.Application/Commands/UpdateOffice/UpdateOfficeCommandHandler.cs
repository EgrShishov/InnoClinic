using InnoClinic.Contracts.OfficeChangedEvent;

public class UpdateOfficeCommandHandler(
    IUnitOfWork unitOfWork, 
    IFilesHttpClient filesHttpClient, 
    IEventBus eventBus) 
    : IRequestHandler<UpdateOfficeCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(UpdateOfficeCommand request, CancellationToken cancellationToken)
    {
        var office = await unitOfWork.OfficeRepository.GetOfficeByIdAsync(request.OfficeId, cancellationToken);
        
        if (office is null)
        {
            return Errors.Offices.NotFound;
        }

        if (request.Photo is not null)
        {
            var photoDeletionResult = await filesHttpClient.DeletedPhoto($"{office.City}-{office.OfficeNumber}");

            if (photoDeletionResult.IsError)
            {
                return Errors.FilesApi.DeletingError;
            }

            var newPhotoUri = await filesHttpClient.UploadPhoto(request.Photo);

            if (newPhotoUri.IsError)
            {
                return Errors.FilesApi.UploadingError;
            }

            office.PhotoId = newPhotoUri.Value;
        }

        office.OfficeNumber = request.OfficeNumber;
        office.HouseNumber = request.HouseNumber;
        office.RegistryPhoneNumber = request.RegistryPhoneNumber;
        office.Street = request.Street;
        office.City = request.City;
        office.IsActive = request.IsActive;

        await unitOfWork.OfficeRepository.UpdateOfficeAsync(office, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await eventBus.PublishAsync(new OfficeChangedEvent
        {
            Id = office.Id.ToString(),
            City = office.City,
            HouseNumber = office.HouseNumber,
            OfficeNumber = office.OfficeNumber,
            PhotoId = office.PhotoId,
            RegistryPhoneNumber = office.RegistryPhoneNumber,
            Street = office.Street
        });

        return Unit.Value;
    }
}
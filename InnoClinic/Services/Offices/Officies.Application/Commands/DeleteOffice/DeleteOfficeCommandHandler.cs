using InnoClinic.Contracts.OfficeDeletedEvent;

public class DeleteOfficeCommandHandler(IUnitOfWork unitOfWork, IFilesHttpClient filesHttpClient, IEventBus eventBus) 
    : IRequestHandler<DeleteOfficeCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(DeleteOfficeCommand request, CancellationToken cancellationToken)
    {
        var office = await unitOfWork.OfficeRepository.GetOfficeByIdAsync(request.Id, cancellationToken);
        
        if (office is null)
        {
            return Errors.Offices.NotFound;
        }

/*        var photoDeletionResult = await filesHttpClient.DeletedPhoto($"{office.City}-{office.OfficeNumber}");
        
        if (photoDeletionResult.IsError)
        {
            return Errors.FilesApi.DeletingError;
        }*/

        await unitOfWork.OfficeRepository.DeleteOfficeAsync(request.Id, cancellationToken);
        
        await unitOfWork.CommitTransactionAsync(cancellationToken);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await eventBus.PublishAsync(new OfficeDeletedEvent
        {
            Id = request.Id.ToString()
        });

        return Unit.Value;
    }
}

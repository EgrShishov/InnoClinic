using InnoClinic.Contracts.OfficeStatusChangedEvent;

public class ChangeOfficesStatusCommandHandler(IUnitOfWork unitOfWork, IEventBus eventBus) 
    : IRequestHandler<ChangeOfficesStatusCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(ChangeOfficesStatusCommand request, CancellationToken cancellationToken)
    {
        var office = await unitOfWork.OfficeRepository.GetOfficeByIdAsync(request.Id, cancellationToken);
        
        if (office == null)
        {
            return Errors.Offices.NotFound;
        }

        office.IsActive = request.isActive;

        await unitOfWork.OfficeRepository.UpdateOfficeAsync(office, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await eventBus.PublishAsync(new OfficeStatusChangedEvent
        {
            Id = request.Id,
            IsActive = office.IsActive
        });

        return Unit.Value;
    }
}

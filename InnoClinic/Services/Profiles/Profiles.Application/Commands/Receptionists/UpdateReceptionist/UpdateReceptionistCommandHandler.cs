public class UpdateReceptionistCommandHandler(
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateReceptionistCommand, ErrorOr<Receptionist>>
{
    public async Task<ErrorOr<Receptionist>> Handle(UpdateReceptionistCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var office = await unitOfWork.OfficeRepository.GetOfficeByIdAsync(request.OfficeId);

            if (office is null)
            {
                return Errors.Office.NotFound;
            }

            var receptionist = await unitOfWork.ReceptionistsRepository.GetReceptionistByIdAsync(request.ReceptionistId);

            if (receptionist is null)
            {
                return Errors.Receptionists.NotFound(request.ReceptionistId);
            }

            receptionist.FirstName = request.FirstName;
            receptionist.LastName = request.LastName;
            receptionist.MiddleName = request.MiddleName;
            receptionist.OfficeId = request.OfficeId;

            await unitOfWork.ReceptionistsRepository.UpdateReceptionistAsync(receptionist);
            await unitOfWork.CompleteAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return receptionist;
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}

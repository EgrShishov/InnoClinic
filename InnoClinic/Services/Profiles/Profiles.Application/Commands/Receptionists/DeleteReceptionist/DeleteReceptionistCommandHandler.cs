    public class DeleteReceptionistCommandHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<DeleteReceptionistCommand, ErrorOr<Unit>>
    {
        public async Task<ErrorOr<Unit>> Handle(DeleteReceptionistCommand request, CancellationToken cancellationToken)
        {
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var receptionist = await unitOfWork.ReceptionistsRepository.GetReceptionistByIdAsync(request.ReceptionistId);

                if (receptionist is null)
                {
                    return Errors.Receptionists.NotFound(request.ReceptionistId);
                }

                await unitOfWork.ReceptionistsRepository.DeleteReceptionistAsync(request.ReceptionistId);
                await unitOfWork.CompleteAsync(cancellationToken);
                await unitOfWork.CommitTransactionAsync(cancellationToken);

                return Unit.Value;
            }
            catch (Exception)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }

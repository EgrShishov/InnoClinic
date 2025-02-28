public class DeleteDoctorCommandHandler(IUnitOfWork unitOfWork, IAccountHttpClient accountHttpClient) 
    : IRequestHandler<DeleteDoctorCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(DeleteDoctorCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var doctor = await unitOfWork.DoctorsRepository.GetDoctorByIdAsync(request.DoctorId);
            
            if (doctor is null)
            {
                return Errors.Doctors.NotFound(request.DoctorId);
            }

            var doctorsAccountDeletionResponse = await accountHttpClient.DeleteAccount(doctor.AccountId);
            
            if (doctorsAccountDeletionResponse.IsError)
            {
                return doctorsAccountDeletionResponse.FirstError;
            }

            await unitOfWork.DoctorsRepository.DeleteDoctorAsync(doctor.Id);
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

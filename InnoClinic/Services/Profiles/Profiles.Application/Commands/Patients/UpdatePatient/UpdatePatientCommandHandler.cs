public class UpdatePatientCommandHandler(IUnitOfWork unitOfWork, IAccountHttpClient accountHttpClient) 
    : IRequestHandler<UpdatePatientCommand, ErrorOr<Patient>>
{
    public async Task<ErrorOr<Patient>> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var patientProfile = await unitOfWork.PatientsRepository.GetPatientByIdAsync(request.PatientId);
            if (patientProfile is null)
            {
                return Errors.Patients.NotFound(request.PatientId);
            }

            var accountInfoResponse = await accountHttpClient.GetAccountInfo(patientProfile.AccountId);
            if (accountInfoResponse.IsError)
            {
                return accountInfoResponse.FirstError;
            }

            patientProfile.FirstName = request.FirstName;
            patientProfile.LastName = request.LastName;
            patientProfile.MiddleName = request.MiddleName;
            patientProfile.DateOfBirth = request.DateOfBirth;

            await unitOfWork.PatientsRepository.UpdatePatientAsync(patientProfile);
            await unitOfWork.CompleteAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return patientProfile;
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}

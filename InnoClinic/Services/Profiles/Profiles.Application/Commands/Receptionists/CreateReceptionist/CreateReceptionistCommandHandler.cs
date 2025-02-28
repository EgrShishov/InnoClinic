public class CreateReceptionistCommandHandler(
    IUnitOfWork unitOfWork,
    IAccountHttpClient accountHttpClient) 
    : IRequestHandler<CreateReceptionistCommand, ErrorOr<CreateReceptionistProfileResponse>>
{
    public async Task<ErrorOr<CreateReceptionistProfileResponse>> Handle(CreateReceptionistCommand request, CancellationToken cancellationToken)
    {
        int accountId = 0;

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var office = await unitOfWork.OfficeRepository.GetOfficeByIdAsync(request.OfficeId);

            if (office is null)
            {
                return Errors.Office.NotFound;
            }

            var createAccountRequest = new CreateAccountRequest
            {
                Email = request.Email,
                Photo = request.Photo,
                PhoneNumber = request.PhoneNumber,
                CreatedBy = request.CreatedBy,
                Role = nameof(Receptionist)
            };

            var response = await accountHttpClient.CreateAccount(createAccountRequest);

            if (response.IsError)
            {
                return response.FirstError;
            }

            var authorizationResponse = response.Value;

            accountId = authorizationResponse.AccountId;

            Receptionist receptionist = new Receptionist
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                OfficeId = request.OfficeId,
                AccountId = accountId
            };

            var newReceptionist = await unitOfWork.ReceptionistsRepository.AddReceptionistAsync(receptionist);

            if (newReceptionist is null)
            {
                return Error.Failure("Cannot add receptionist to repository");
            }

            await unitOfWork.CompleteAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return new CreateReceptionistProfileResponse
            {
                AccountId = newReceptionist.AccountId,
                Email = request.Email,
                ReceptionistId = newReceptionist.Id
            };
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);

            if (accountId != 0)
            {
                await accountHttpClient.DeleteAccount(accountId);
            }

            throw;
        }
    }
}

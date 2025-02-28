public class CreateDoctorCommandHandler(
    IUnitOfWork unitOfWork, 
    IAccountHttpClient accountHttpClient) 
    : IRequestHandler<CreateDoctorCommand, ErrorOr<Doctor>>
{
    public async Task<ErrorOr<Doctor>> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
    {
        int accountId = 0;

        try
        {
            var createDoctorsAccountResponse = await accountHttpClient.CreateAccount(
                new CreateAccountRequest
                {
                    Email = request.Email,
                    Role = nameof(Doctor),
                    PhoneNumber = string.Empty,
                    Photo = request.Photo,
                    CreatedBy = request.CreatedBy
                });

            if (createDoctorsAccountResponse.IsError)
            {
                return createDoctorsAccountResponse.FirstError;
            }

            var authorizationResponse = createDoctorsAccountResponse.Value;

            accountId = authorizationResponse.AccountId;

            Doctor doctor = new Doctor
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                SpecializationId = request.SpecializationId,
                DateOfBirth = request.DateOfBirth,
                CareerStartYear = request.CareerStartYear,
                OfficeId = request.OfficeId,
                Status = request.Status,
                AccountId = accountId
            };

            var addedDoctor = await unitOfWork.DoctorsRepository.AddDoctorAsync(doctor);

            await unitOfWork.CompleteAsync(cancellationToken);

            return addedDoctor;
        }
        catch (Exception)
        {
            if (accountId != 0)
            {
                await accountHttpClient.DeleteAccount(accountId);
            }
            throw;
        }
    }
}

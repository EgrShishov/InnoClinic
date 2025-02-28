public class CreatePatientCommandHandler(IUnitOfWork unitOfWork, IAccountHttpClient accountHttpClient) 
    : IRequestHandler<CreatePatientCommand, ErrorOr<CreatePatientResponse>>
{
    public async Task<ErrorOr<CreatePatientResponse>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
    {
        var existingProfiles = await unitOfWork
            .PatientsRepository
            .GetListPatientsAsync(cancellationToken);

        if (existingProfiles is not null && existingProfiles.Any())
        {
            var matchedProfile = existingProfiles
                .Where(p => !p.IsLinkedToAccount) //problem here is that we create profile with account
                .Select(p => new
                {
                    Profile = p,
                    MatchScore = CalculateMatchScore(p, request)
                })
                .OrderByDescending(p => p.MatchScore)
                .FirstOrDefault(p => p.MatchScore >=13).Profile;

            if (matchedProfile is not null)
            {
                return new CreatePatientResponse
                {
                    Success = false,
                    IsMatchFound = true,
                    Message = "A similar profile has been found",
                    MatchedProfile = new PatientProfileResponse
                    {
                        UserId = matchedProfile.Id,
                        FirstName = matchedProfile.FirstName,
                        LastName = matchedProfile.LastName,
                        MiddleName = matchedProfile.MiddleName,
                        DateOfBirth = matchedProfile.DateOfBirth
                    }
                };
            }
        }

        var createPatientAccountResponse = await accountHttpClient.CreateAccount(
           new CreateAccountRequest
           {
               Email = request.Email,
               PhoneNumber = request.PhoneNumber,
               Role = nameof(Patient),
               Photo = request.Photo,
           });

        if (createPatientAccountResponse.IsError)
        {
            return createPatientAccountResponse.FirstError;
        }

        var response = createPatientAccountResponse.Value;

        Patient patient = new Patient
        {
            AccountId = response.AccountId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            MiddleName = request.MiddleName,
            DateOfBirth = request.DateOfBirth,
            IsLinkedToAccount = true
        };

        var newPatient = await unitOfWork.PatientsRepository.AddPatientAsync(patient);
        
        await unitOfWork.CompleteAsync(cancellationToken);

        return new CreatePatientResponse
        {
            Id = newPatient.Id,
            Success = true,
            IsMatchFound = false
        };
    }

    private int CalculateMatchScore(Patient existingProfile, CreatePatientCommand request)
    {
        int score = 0;

        if (existingProfile.FirstName == request.FirstName) score += 5;
        if (existingProfile.LastName == request.LastName) score += 5;
        if (existingProfile.MiddleName == request.MiddleName) score += 5;
        if (existingProfile.DateOfBirth == request.DateOfBirth) score += 3;
        
        return score;
    }
}

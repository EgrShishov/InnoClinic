public class ViewPatientProfileQueryHandler(IUnitOfWork unitOfWork, IAccountHttpClient accountHttpClient)
    : IRequestHandler<ViewPatientProfileQuery, ErrorOr<PatientProfileResponse>>
{
    public async Task<ErrorOr<PatientProfileResponse>> Handle(ViewPatientProfileQuery request, CancellationToken cancellationToken)
    {
        var patient = await unitOfWork.PatientsRepository.GetPatientByIdAsync(request.PatientId, cancellationToken);

        if (patient is null)
        {
            return Errors.Patients.NotFound(request.PatientId);
        }
        
        var accountResponse = await accountHttpClient.GetAccountInfo(patient.AccountId);
        
        if (accountResponse.IsError)
        {
            return accountResponse.FirstError;
        }

        var account = accountResponse.Value;

        return new PatientProfileResponse
        {
            UserId = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            MiddleName = patient.MiddleName,
            DateOfBirth = patient.DateOfBirth,
            PhoneNumber = account.PhoneNumber,
            PhotoUrl = account.PhotoUrl
        };
    }
}
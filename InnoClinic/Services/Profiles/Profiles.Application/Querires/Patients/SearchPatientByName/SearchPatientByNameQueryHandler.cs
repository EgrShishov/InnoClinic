public class SearchPatientByNameQueryHandler(IUnitOfWork unitOfWork, IAccountHttpClient accountHttpClient) 
    : IRequestHandler<SearchPatientByNameQuery, ErrorOr<List<PatientListResponse>>>
{
    public async Task<ErrorOr<List<PatientListResponse>>> Handle(SearchPatientByNameQuery request, CancellationToken cancellationToken)
    {
        var patients = await unitOfWork
            .PatientsRepository
                .ListPatientsAsync(p => p.FirstName == request.FirstName
                                    && p.LastName == request.LastName
                                    && p.MiddleName == request.MiddleName,
                                    cancellationToken);

        if (patients is null || !patients.Any())
        {
            return Errors.Patients.NotFoundByFullName;
        }

        var patientList = new List<PatientListResponse>();
        
        foreach (var patient in patients)
        {
            var accountResponse = await accountHttpClient.GetAccountInfo(patient.AccountId);

            if (accountResponse.IsError)
            {
                return accountResponse.FirstError;
            }

            var account = accountResponse.Value;

            patientList.Add(new PatientListResponse
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                MiddleName = patient.MiddleName,
                PhoneNumber = account.PhoneNumber
            });
        }

        return patientList;
    }
}

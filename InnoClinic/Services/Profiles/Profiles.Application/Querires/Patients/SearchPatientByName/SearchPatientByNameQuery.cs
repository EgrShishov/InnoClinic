public sealed record SearchPatientByNameQuery(string FirstName, string LastName, string MiddleName) 
    : IRequest<ErrorOr<List<PatientListResponse>>>
{
}

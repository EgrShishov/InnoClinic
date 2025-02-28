public sealed record UpdatePatientCommand(
    int PatientId,
    string FirstName,
    string LastName,
    string MiddleName,
    string PhoneNumber,
    DateTime DateOfBirth) : IRequest<ErrorOr<Patient>>
{ }

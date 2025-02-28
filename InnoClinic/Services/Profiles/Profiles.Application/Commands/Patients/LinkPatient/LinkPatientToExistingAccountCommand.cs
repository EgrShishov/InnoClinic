public sealed record LinkPatientToExistingAccountCommand(int AccountId, int PatientProfileId) : IRequest<ErrorOr<Unit>>
{
}

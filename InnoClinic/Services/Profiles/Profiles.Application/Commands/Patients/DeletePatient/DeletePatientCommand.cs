public sealed record DeletePatientCommand(int PatientId) : IRequest<ErrorOr<Unit>>
{
}

public sealed record ViewPatientProfileQuery(int PatientId) : IRequest<ErrorOr<PatientProfileResponse>>
{
}

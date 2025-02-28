public sealed record ViewAllPatientsQuery(int PageSize, int PageNumber) : IRequest<ErrorOr<List<PatientListResponse>>>
{
}

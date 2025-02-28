public sealed record ViewAllReceptionistsQuery(int PageNumber, int PageSize) : IRequest<ErrorOr<List<ReceptionistListResponse>>>
{
}

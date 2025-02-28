public sealed record GetOfficesQuery() : IRequest<ErrorOr<List<OfficeListResponse>>>
{
}

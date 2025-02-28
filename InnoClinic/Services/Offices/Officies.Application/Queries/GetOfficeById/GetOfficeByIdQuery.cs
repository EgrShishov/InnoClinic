public sealed record GetOfficeByIdQuery(string Id) : IRequest<ErrorOr<OfficeResponse>>
{
}

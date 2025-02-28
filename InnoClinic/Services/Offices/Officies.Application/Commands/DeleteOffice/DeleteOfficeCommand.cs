public sealed record DeleteOfficeCommand(string Id) : IRequest<ErrorOr<Unit>>
{
}

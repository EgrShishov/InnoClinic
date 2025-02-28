public record ChangeOfficesStatusCommand(string Id, bool isActive) : IRequest<ErrorOr<Unit>>
{
}

public record DeleteServiceCommand(int ServiceId) : IRequest<ErrorOr<Unit>>
{
}
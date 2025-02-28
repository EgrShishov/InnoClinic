public record DeleteSpecializationCommand(int Id) : IRequest<ErrorOr<Unit>>
{
}

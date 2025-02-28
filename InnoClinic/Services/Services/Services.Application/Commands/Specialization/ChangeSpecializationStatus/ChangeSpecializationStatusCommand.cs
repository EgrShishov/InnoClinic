public sealed record ChangeSpecializationStatusCommand(int Id, bool IsActive) : IRequest<ErrorOr<SpecializationResponse>>
{
}

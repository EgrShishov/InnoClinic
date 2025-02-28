public sealed record CreateSpecializationCommand(string SpecializationName, bool IsActive) : IRequest<ErrorOr<SpecializationResponse>>
{
}


public sealed record UpdateSpecializationCommand(int Id, string SpecializationName, bool IsActive) 
    : IRequest<ErrorOr<SpecializationInfoResponse>>
{
}

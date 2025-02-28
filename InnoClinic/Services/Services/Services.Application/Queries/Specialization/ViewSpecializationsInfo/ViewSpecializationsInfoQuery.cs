public sealed record ViewSpecializationsInfoQuery(int Id) : IRequest<ErrorOr<SpecializationInfoResponse>>
{
}
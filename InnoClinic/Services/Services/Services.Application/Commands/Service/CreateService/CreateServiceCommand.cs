public sealed record CreateServiceCommand(
    ServiceCategory ServiceCategory,
    string ServiceName,
    Decimal ServicePrice,
    bool IsActive,
    int SpecializationId) : IRequest<ErrorOr<ServiceInfoResponse>>
{
}

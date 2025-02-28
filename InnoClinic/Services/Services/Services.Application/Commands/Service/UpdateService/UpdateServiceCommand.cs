public sealed record UpdateServiceCommand(
    int Id, 
    ServiceCategory ServiceCategory,
    string ServiceName,
    Decimal ServicePrice,
    bool IsActive) : IRequest<ErrorOr<ServiceInfoResponse>>
{
}

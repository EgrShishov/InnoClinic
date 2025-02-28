public sealed record ViewServicesInfoQuery(int Id) : IRequest<ErrorOr<ServiceInfoResponse>>
{
}


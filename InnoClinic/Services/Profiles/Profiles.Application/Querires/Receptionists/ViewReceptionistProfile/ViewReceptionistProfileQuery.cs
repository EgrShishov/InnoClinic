public sealed record ViewReceptionistProfileQuery(int ReceptionistId) : IRequest<ErrorOr<ReceptionistProfileInfoResponse>>
{
}

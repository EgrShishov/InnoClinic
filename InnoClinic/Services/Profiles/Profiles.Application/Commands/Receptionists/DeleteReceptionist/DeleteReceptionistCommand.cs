public sealed record DeleteReceptionistCommand(int ReceptionistId) : IRequest<ErrorOr<Unit>>
{
}


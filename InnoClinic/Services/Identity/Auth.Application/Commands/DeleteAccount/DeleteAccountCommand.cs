public sealed record DeleteAccountCommand(int AccountId) : IRequest<ErrorOr<Unit>>
{
}
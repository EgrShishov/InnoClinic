public record VerifyEmailCommand(int AccountId, string Link) : IRequest<ErrorOr<Unit>> 
{
}

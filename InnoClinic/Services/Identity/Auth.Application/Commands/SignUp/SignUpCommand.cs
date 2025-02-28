public sealed record SignUpCommand(string Email, string Password, string PhoneNumber) : IRequest<ErrorOr<AuthorizationResponse>>
{
}

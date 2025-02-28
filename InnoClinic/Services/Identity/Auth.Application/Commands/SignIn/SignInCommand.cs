
public sealed record SignInCommand(string Email, string Password, string Role) 
    : IRequest<ErrorOr<AuthorizationResponse>> { }

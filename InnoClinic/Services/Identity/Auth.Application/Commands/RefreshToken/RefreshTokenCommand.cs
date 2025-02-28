public sealed record RefreshTokenCommand(string AccessToken, string RefreshToken) : IRequest<ErrorOr<RefreshTokenResponse>>
{
}

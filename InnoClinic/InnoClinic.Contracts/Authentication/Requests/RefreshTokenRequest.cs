
namespace InnoClinic.Contracts.Authentication.Requests
{
    public record RefreshTokenRequest(string accessToken, string refreshToken);
}

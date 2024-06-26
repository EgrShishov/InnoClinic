
namespace InnoClinic.Contracts.Authentication.Responses
{
    public record AuthorizationResponse(string accessToken, string refreshToken, string role);
}

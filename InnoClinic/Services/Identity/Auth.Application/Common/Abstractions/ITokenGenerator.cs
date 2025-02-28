using System.Security.Claims;

public interface ITokenGenerator
{
    public string GenerateAccessToken(Account account);
    public string GenerateRefreshToken();
    public ClaimsPrincipal GetPrincipalFromToken(string token);
}

using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

public class RefreshTokenCommandHandler(UserManager<Account> userManager, ITokenGenerator tokenGenerator)
    : IRequestHandler<RefreshTokenCommand, ErrorOr<RefreshTokenResponse>>
{
    public async Task<ErrorOr<RefreshTokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var principal = tokenGenerator.GetPrincipalFromToken(request.AccessToken);

        var accountId = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        
        var account = await userManager.FindByIdAsync(accountId);

        if (account == null || account.RefreshToken != request.RefreshToken)
        {
            return Errors.Authentication.InvalidRefreshToken;
        }

        var newAccessToken = tokenGenerator.GenerateAccessToken(account);
        var newRefreshToken = tokenGenerator.GenerateRefreshToken();

        account.RefreshToken = newRefreshToken;
        await userManager.UpdateAsync(account);

        return new RefreshTokenResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }
}

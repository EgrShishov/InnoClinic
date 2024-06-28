using Auth.Application.Common;
using InnoClinic.Contracts.Authentication.Responses;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace Auth.Application.Commands.Refresh
{
    public sealed record RefreshTokenCommand(string AccessToken, string RefreshToken) : IRequest<ErrorOr<RefreshTokenResponse>>
    {
    }

    public class RefreshTokenCommandHandler(UserManager<Account> userManager, ITokenGenerator tokenGenerator) : IRequestHandler<RefreshTokenCommand, ErrorOr<RefreshTokenResponse>>
    {
        public async Task<ErrorOr<RefreshTokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var principal = tokenGenerator.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal == null)
            {
                return Errors.Authentication.InvalidToken;
            }

            var email = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;
            if (email == null)
            {
                return Errors.Authentication.InvalidToken;
            }

            var account = await userManager.FindByEmailAsync(email);
            if (account == null || !account.RefreshTokens.Any(rt => rt == request.RefreshToken))
            {
                return Errors.Authentication.InvalidToken;
            }

            var newAccessToken = tokenGenerator.GenerateAccessToken(account);
            var newRefreshToken = tokenGenerator.GenerateRefreshToken(account);

            account.RefreshTokens.Remove(request.RefreshToken);
            account.RefreshTokens.Add(newRefreshToken);

            return new RefreshTokenResponse(newAccessToken, newRefreshToken);
        }
    }
}

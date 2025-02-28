using Microsoft.AspNetCore.Identity;

public class SignInCommandHandler(
    UserManager<Account> manager,
    ITokenGenerator tokenService) : IRequestHandler<SignInCommand, ErrorOr<AuthorizationResponse>>
{
    public async Task<ErrorOr<AuthorizationResponse>> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var account = await manager.FindByEmailAsync(request.Email);
        
        if (account is null)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var isPasswordValid = await manager.CheckPasswordAsync(account, request.Password);
        
        if (!isPasswordValid)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var accessToken = tokenService.GenerateAccessToken(account);
        var refreshToken = tokenService.GenerateRefreshToken();

        account.RefreshToken = refreshToken;
        await manager.UpdateAsync(account);

        return new AuthorizationResponse
        {
            AccountId = account.Id,
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}

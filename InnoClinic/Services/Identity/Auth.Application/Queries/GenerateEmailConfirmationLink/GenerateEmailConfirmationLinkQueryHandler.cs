using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;

public class GenerateEmailConfirmationLinkQueryHandler(
        UserManager<Account> userManager,
        IConfiguration configuration) : IRequestHandler<GenerateEmailConfirmationLinkQuery, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(GenerateEmailConfirmationLinkQuery request, CancellationToken cancellationToken)
    {
        var account = await userManager.FindByIdAsync(request.AccountId.ToString());
        
        if (account is null)
        {
            return Errors.Account.NotFound(request.AccountId);
        }

        var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(account);

        var baseUrl = configuration["AppSettings:BaseUrl"];
        var emailConfirmationLink = $"{baseUrl}/api/account/confirm-email?token={Uri.EscapeDataString(confirmationToken)}";
        
        return emailConfirmationLink;
    }
}

using Auth.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Auth.Application.Queries
{
    public sealed record GenerateEmailConfirmationLinkQuery(Account account) : IRequest<ErrorOr<string>>
    {
    }

    public class GenerateEmailConfirmationLinkQueryHandler(UserManager<Account> userManager, IEmailSender emailSender, IConfiguration configuration)
        : IRequestHandler<GenerateEmailConfirmationLinkQuery, ErrorOr<string>>
    {
        public async Task<ErrorOr<string>> Handle(GenerateEmailConfirmationLinkQuery request, CancellationToken cancellationToken)
        {
            var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(request.account);

            var baseUrl = configuration["AppSettings:BaseUrl"];
            var emailConfirmationLink = $"{baseUrl}/api/account/confirm-email?userId={request.account.Id}&token={Uri.EscapeDataString(confirmationToken)}";
            return emailConfirmationLink;
        }
    }
}

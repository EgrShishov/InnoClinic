using Microsoft.AspNetCore.Identity;

public sealed class SignUpCommandHandler(
    UserManager<Account> manager,
    IMediator mediator,
    IEmailSender emailSender,
    ITokenGenerator tokenGenerator) : IRequestHandler<SignUpCommand, ErrorOr<AuthorizationResponse>>
{
    public async Task<ErrorOr<AuthorizationResponse>> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        var account = await manager.FindByEmailAsync(request.Email);

        if (account is not null)
        {
            return Errors.Account.DuplicateEmail;
        }

        var newAccount = new Account
        {
            Email = request.Email,
            CreatedAt = DateTime.UtcNow,
            UserName = request.Email,
            PhoneNumber = request.PhoneNumber,
        };

        var identityResult = await manager.CreateAsync(newAccount, request.Password);

        if (!identityResult.Succeeded)
        {
            return Errors.Authentication.GenerationFailed;
        }

        await manager.AddToRoleAsync(newAccount, "Patient");

        var confirmationLink = await mediator.Send(new GenerateEmailConfirmationLinkQuery(newAccount.Id));

        var emailTemplate = new EmailTemplates.EmailConfirmationLinkTemplate
        {
            ConfirmationLink = confirmationLink.Value
        };

        await emailSender.SendEmailAsync(request.Email, "Confirm your email", emailTemplate.GetContent());

        var accessToken = tokenGenerator.GenerateAccessToken(newAccount);
        var refreshToken = tokenGenerator.GenerateRefreshToken();

        newAccount.RefreshToken = refreshToken;
        await manager.UpdateAsync(newAccount);

        return new AuthorizationResponse
        {
            AccountId = newAccount.Id,
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}
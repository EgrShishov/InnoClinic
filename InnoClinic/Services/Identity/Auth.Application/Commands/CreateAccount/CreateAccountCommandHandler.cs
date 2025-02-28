using Microsoft.AspNetCore.Identity;

public class CreateAccountCommandHandler(
    IEmailSender emailSender,
    IFilesHttpClient filesHttpClient,
    IPasswordGenerator passwordGenerator,
    UserManager<Account> manager,
    ITokenGenerator tokenGenerator) : IRequestHandler<CreateAccountCommand, ErrorOr<AuthorizationResponse>>
{
    public async Task<ErrorOr<AuthorizationResponse>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await manager.FindByEmailAsync(request.Email);

        if (account is not null)
        {
            return Errors.Account.DuplicateEmail;
        }

        var fileResponse = await filesHttpClient.UploadPhoto(new UploadPhotoRequest { File = request.Photo });

        if (fileResponse.IsError)
        {
            return fileResponse.FirstError;
        }

        var generatedPassword = passwordGenerator.GeneratePassword(15, 5);

        var newAccount = new Account
        {
            Email = request.Email,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = request.CreatedBy,
            UserName = request.Email,
            PhoneNumber = request.PhoneNumber,
            PhotoUrl = fileResponse.Value,
            EmailConfirmed = true
        };

        var identityResult = await manager.CreateAsync(newAccount, generatedPassword);
        
        if (!identityResult.Succeeded)
        {
            return Errors.Account.CreationFailed(identityResult.Errors.First().Description);
        }

        await manager.AddToRoleAsync(newAccount, request.Role);
        
        var emailTemplate = new EmailTemplates.CredentialsEmailTemplate
        {
            Email = request.Email,
            Password = generatedPassword
        };

        await emailSender.SendEmailAsync(request.Email, "Your credentials", emailTemplate.GetContent());

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

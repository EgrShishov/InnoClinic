using Microsoft.AspNetCore.Identity;

public class VerifyEmailCommandHandler(UserManager<Account> manager) 
    : IRequestHandler<VerifyEmailCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var account = await manager.FindByIdAsync(request.AccountId.ToString());
       
        if (account is null)
        {
            return Errors.Account.NotFound(request.AccountId);
        }

        var identityResult = await manager.ConfirmEmailAsync(account, request.Link);
       
        if (!identityResult.Succeeded)
        {
            return Errors.Authentication.InvalidEmailVerificationLink;
        }

        return Unit.Value;
    }
}
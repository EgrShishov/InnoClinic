using Microsoft.AspNetCore.Identity;

public sealed class DeleteAccountCommandHandler(UserManager<Account> userManager) : IRequestHandler<DeleteAccountCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await userManager.FindByIdAsync(request.AccountId.ToString());

        if (account is null)
        {
            return Errors.Account.NotFound(request.AccountId);
        }

        var identityResponse = await userManager.DeleteAsync(account);

        if (!identityResponse.Succeeded)
        {
            return Errors.Account.DeleteFailed;
        }

        return Unit.Value;
    }
}
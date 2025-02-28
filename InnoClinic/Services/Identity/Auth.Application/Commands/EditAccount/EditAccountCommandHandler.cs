using Microsoft.AspNetCore.Identity;

public sealed class EditAccountCommandHandler(
    UserManager<Account> userManager,
    IFilesHttpClient filesHttpClient) : IRequestHandler<EditAccountCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(EditAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await userManager.FindByIdAsync(request.AccountId.ToString());
        
        if (account is null)
        {
            return Errors.Account.NotFound(request.AccountId);
        }

        var accountUpdatedByUser = await userManager.FindByIdAsync(request.UpdatedBy.ToString());

        if (accountUpdatedByUser is null)
        {
            return Errors.Account.NotFound(request.UpdatedBy);
        }

        var fileResponse = await filesHttpClient.UploadPhoto(new UploadPhotoRequest { File = request.Photo });

        if (fileResponse.IsError)
        {
            return fileResponse.FirstError;
        }

        account.PhoneNumber = request.PhoneNumber;
        account.PhotoUrl = fileResponse.Value;
        account.UpdatedAt = DateTime.UtcNow;
        account.UpdatedBy = request.UpdatedBy;

        var updatingResult = await userManager.UpdateAsync(account);

        if (!updatingResult.Succeeded)
        {
            return Errors.Account.UpdateFailed;
        }

        return Unit.Value;
    }
}
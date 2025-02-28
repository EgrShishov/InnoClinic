using Microsoft.AspNetCore.Identity;

public class GetAccountByIdQueryHandler(UserManager<Account> manager) : IRequestHandler<GetAccountByIdQuery, ErrorOr<AccountResponse>>
{
    public async Task<ErrorOr<AccountResponse>> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var account = await manager.FindByIdAsync(request.id.ToString());
        
        if (account is null)
        {
            return Errors.Account.NotFound(request.id);
        }

        return new AccountResponse
        {
            AccountId = account.Id,
            CreatedAt = account.CreatedAt,
            UpdatedAt = account.UpdatedAt,
            CreatedBy = account.CreatedBy,
            UpdatedBy = account.UpdatedBy,
            Email = account.Email,
            PhoneNumber = account.PhoneNumber,
            PhotoUrl = account.PhotoUrl
        };
    }
}

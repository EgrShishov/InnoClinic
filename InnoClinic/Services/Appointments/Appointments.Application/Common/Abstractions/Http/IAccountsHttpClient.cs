public interface IAccountsHttpClient
{
    public Task<ErrorOr<AccountResponse>> GetAccountInfoAsync(int AccountId);
}
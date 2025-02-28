using ErrorOr;
using System.Net.Http.Json;

public class AccountHttpClient : IAccountsHttpClient
{
    private readonly IHttpClientFactory _factory;
    private readonly HttpClient _httpClient;
    public AccountHttpClient(IHttpClientFactory factory)
    {
        _factory = factory;
        _httpClient = _factory.CreateClient("identity");
    }

    public async Task<ErrorOr<AccountResponse>> GetAccountInfoAsync(int AccountId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Authorization/account/{AccountId}");

            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                return Error.Failure(description: responseBody);
            }

            return await response.Content.ReadFromJsonAsync<AccountResponse>();
        }
        catch (Exception ex)
        {
            return Error.Failure(code: "An error occured while getting account info", description: ex.Message);
        }
    }
}

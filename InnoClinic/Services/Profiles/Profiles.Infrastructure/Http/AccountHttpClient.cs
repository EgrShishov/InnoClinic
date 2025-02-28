using ErrorOr;
using MediatR;
using System.Net.Http.Headers;
using System.Net.Http.Json;

public class AccountHttpClient : IAccountHttpClient
{
    private readonly IHttpClientFactory _factory;
    public AccountHttpClient(IHttpClientFactory factory)
    {  
        _factory = factory;
    }

    public async Task<ErrorOr<AuthorizationResponse>> CreateAccount(CreateAccountRequest request)
    {
        try
        {
            var _httpClient = _factory.CreateClient("identity");

            MultipartFormDataContent formData = new MultipartFormDataContent();

            if (request.Photo != null)
            {
                var streamContent = new StreamContent(request.Photo.OpenReadStream());
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(request.Photo.ContentType);
                formData.Add(streamContent, nameof(request.Photo), request.Photo.FileName);
            }

            formData.Add(new StringContent(request.Email), nameof(request.Email));
            formData.Add(new StringContent(request.PhoneNumber), nameof(request.PhoneNumber));
            formData.Add(new StringContent(request.Role), nameof(request.Role));
            formData.Add(new StringContent(request.CreatedBy.ToString()), nameof(request.CreatedBy));

            var response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/Authorization/create", formData);

            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                return Error.Failure(description: responseBody);
            }

            return await response.Content.ReadFromJsonAsync<AuthorizationResponse>();

        }
        catch (Exception ex)
        {
            return Error.Failure(code: "An error occurred while creating account", description: ex.Message);
        }
    }

    public async Task<ErrorOr<Unit>> DeleteAccount(int AccountId)
    {
        var _httpClient = _factory.CreateClient("identity");

        try
        {
            var response = await _httpClient.DeleteAsync($"{_httpClient.BaseAddress}/Authorization/account/{AccountId}");

            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                return Error.Failure(description: responseBody);
            }

            return Unit.Value;
        }
        catch (Exception ex)
        {
            return Error.Failure(code: "An error occured while deleting account", description: ex.Message);
        }
    }

    public async Task<ErrorOr<AccountResponse>> GetAccountInfo(int AccountId)
    {
        var _httpClient = _factory.CreateClient("identity");

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
using Microsoft.AspNetCore.Http;

public class FilesHttpClient : IFilesHttpClient
{
    private readonly IHttpClientFactory _factory;
    public FilesHttpClient(IHttpClientFactory factory)
    {
        _factory = factory;
    }

    public async Task<ErrorOr<Unit>> DeletedPhoto(string fileName)
    {
        var _httpClient = _factory.CreateClient("files");

        var response = await _httpClient.DeleteAsync($"{_httpClient.BaseAddress}/photo/{fileName}");
        
        if (!response.IsSuccessStatusCode)
        {
            return Error.Failure();
        }
        
        return Unit.Value;
    }

    public async Task<ErrorOr<string>> UploadPhoto(IFormFile photo)
    {
        var _httpClient = _factory.CreateClient("files");

        using var content = new MultipartFormDataContent();

        var fileContent = new StreamContent(photo.OpenReadStream());

        var contentType = string.IsNullOrWhiteSpace(photo.ContentType) ? "application/octet-stream" : photo.ContentType;
        
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
            
        content.Add(fileContent, photo.Name, photo.FileName);

        var response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/photos", content);

        if (!response.IsSuccessStatusCode)
        {
            return Error.Failure("Failed to upload photo");
        }

        return await response.Content.ReadAsStringAsync();
    }
}

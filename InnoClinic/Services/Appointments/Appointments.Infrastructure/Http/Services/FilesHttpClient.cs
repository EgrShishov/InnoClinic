using ErrorOr;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

public class FilesHttpClient : IFilesHttpClient
{
    private readonly IHttpClientFactory _factory;
    public FilesHttpClient(IHttpClientFactory factory)
    {
        _factory = factory;
    }

    public async Task<ErrorOr<IFormFile>> GetDocumentForResultAsync(int ResultsId)
    {
        var _httpClient = _factory.CreateClient("files");

        try
        {
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/documents/by-result/{ResultsId}");

            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                return Error.Failure(
                    code: "Error in getting documents for result", 
                    description: $"Error in getting documents for result in httpclient : {responseBody}");
            }

            var contentStream = await response.Content.ReadAsStreamAsync();
            var fileName = response.Content.Headers.ContentDisposition?.FileName?.Trim('"') ?? $"results-{ResultsId}";
            var contentType = response.Content.Headers.ContentType?.MediaType ?? "application/octet-stream";

            using var memoryStream = new MemoryStream();
            await contentStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            return new FormFile(memoryStream, 0, memoryStream.Length, fileName, fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }
        catch (Exception ex)
        {
            return Error.Failure(
                code: "An error occured while getting results document", 
                description: $"Some error occured in http client: {ex.Message}");
        }
    }
    public async Task<ErrorOr<string>> UploadDocumentAsync(IFormFile file, int ResultId)
    {
        var _httpClient = _factory.CreateClient("files");

        try
        {
            if (file == null || file.Length == 0)
            {
                return Error.Failure(description: "File must be not empty or null");
            }

            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(file.OpenReadStream());
            var contentType = "application/pdf";
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            content.Add(fileContent, "File", file.FileName);
            content.Add(new StringContent(ResultId.ToString()), "ResultId");

            Console.WriteLine("mama4");
            var response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/document", content);

            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                return Error.Failure(
                    code: "Error in uploading results document",
                    description: $"Error in uploading results document http client: {responseBody}");
            }

            Console.WriteLine("mama5");
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            return Error.Failure(
                code: "An error occured while getting account info", 
                description: $"Some error occured in http client: {ex.Message}");
        }
    }
}

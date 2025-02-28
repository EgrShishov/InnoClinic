using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

public class FilesHttpClient : IFilesHttpClient
{
    private readonly IHttpClientFactory _factory;
    private readonly HttpClient _httpClient;
    public FilesHttpClient(IHttpClientFactory factory)
    {
        _factory = factory;
        _httpClient = _factory.CreateClient("files");
    }

    public async Task<ErrorOr<Unit>> DeleteDocument(string fileName)
    {
        try
        {
            var response = await _httpClient
                 .DeleteAsync($"{_httpClient.BaseAddress}/file/document/{fileName}");

            if (!response.IsSuccessStatusCode)
            {
                return Error.Failure();
            }

            return Unit.Value;
        }
        catch (HttpRequestException ex)
        {
            return Error.Failure(code: "Request error occurred while deleting document", description: ex.Message);
        }
        catch (Exception ex)
        {
            return Error.Failure(code: "Unexpected error occurred while deleting document", description: ex.Message);
        }
    }

    public async Task<ErrorOr<Unit>> DeletePhoto(string fileName)
    {
        try
        {
            var response = await _httpClient
                .DeleteAsync($"{_httpClient.BaseAddress}/file/photo/{fileName}");

            if (!response.IsSuccessStatusCode)
            {
                return Error.Failure("Failed to delete photo");
            }

            return Unit.Value;
        }
        catch (HttpRequestException ex)
        {
            return Error.Failure(code: "Request error occurred while deleting photo", description: ex.Message);
        }
        catch (Exception ex)
        {
            return Error.Failure(code: "Unexpected error occurred while deleting photo", description: ex.Message);
        }
    }

    public async Task<ErrorOr<FileResponse>> DownloadDocument(string fileName)
    {
        try
        {
            var response = await _httpClient
                .GetAsync($"{_httpClient.BaseAddress}/file/document/{fileName}");

            if (!response.IsSuccessStatusCode)
            {
                return Error.Failure("Failed to download document");
            }

            using MemoryStream stream = new();

            await response.Content.CopyToAsync(stream);

            var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/octet-stream";

            return new FileResponse
            {
                Content = stream,
                ContentType = contentType,
                Filename = fileName
            };
        }
        catch (HttpRequestException ex)
        {
            return Error.Failure(code: "Request error occurred while downloading document", description: ex.Message);
        }
        catch (Exception ex)
        {
            return Error.Failure(code: "Unexpected error occurred while downloading document", description: ex.Message);
        }
    }

    public async Task<ErrorOr<string>> UploadDocument(UploadDocumentRequest request)
    {
        try
        {
            var content = GetMultipartFormDataContent(request.File);

            var response = await _httpClient
                .PostAsync($"{_httpClient.BaseAddress}/file/document", content);

            if (!response.IsSuccessStatusCode)
            {
                return Error.Failure("Failed to upload document");
            }

            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException ex)
        {
            return Error.Failure(code: "Request error occurred while uploading document", description: ex.Message);
        }
        catch (Exception ex)
        {
            return Error.Failure(code: "Unexpected error occurred while uploading document", description: ex.Message);
        }
    }

    public async Task<ErrorOr<string>> UploadPhoto(UploadPhotoRequest request)
    {
        try
        {
            var content = GetMultipartFormDataContent(request.File);

            var response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/photos", content);

            if (!response.IsSuccessStatusCode)
            {
                return Error.Failure("Failed to upload photo");
            }

            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException ex)
        {
            return Error.Failure(code: "Request error occurred while uploading photo", description: ex.Message);
        }
        catch (Exception ex)
        {
            return Error.Failure(code: "Unexpected error occurred while uploading photo", description: ex.Message);
        }
    }

    private MultipartFormDataContent GetMultipartFormDataContent(IFormFile file)
    {
        var content = new MultipartFormDataContent();
        var fileContent = new StreamContent(file.OpenReadStream());
        var contentType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType;

        fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
        content.Add(fileContent, file.Name, file.FileName);

        return content;
    }
}

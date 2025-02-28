using Microsoft.AspNetCore.Http;

public interface IFilesHttpClient
{
    public Task<ErrorOr<IFormFile>> GetDocumentForResultAsync(int ResultsId);
    public Task<ErrorOr<string>> UploadDocumentAsync(IFormFile file, int ResultsId);
}
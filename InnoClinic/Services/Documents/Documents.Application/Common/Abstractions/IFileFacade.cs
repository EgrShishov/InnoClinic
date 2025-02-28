using Microsoft.AspNetCore.Http;

public interface IFileFacade
{
    public Task<ErrorOr<string>> UploadDocumentAsync(IFormFile file, int resultId);
    public Task<ErrorOr<FileResponse>> DownloadDocumentAsync(int resultId);
    public Task<ErrorOr<Unit>> DeleteResultDocumentAsync(int resultId);
    public Task<ErrorOr<string>> UploadPhotoAsync(IFormFile file);
    public Task<ErrorOr<FileResponse>> DownloadPhotoAsync(string url);
    public Task<ErrorOr<Unit>> DeletePhotoAsync(string url);
}

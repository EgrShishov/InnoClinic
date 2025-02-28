using Microsoft.AspNetCore.Http;

public interface IFilesHttpClient
{
    public Task<ErrorOr<string>> UploadPhoto(IFormFile photo);
    public Task<ErrorOr<Unit>> DeletedPhoto(string fileName);
}

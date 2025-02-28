using Microsoft.AspNetCore.Http;

public class UploadPhotoRequest
{
    public IFormFile File { get; init; }
}
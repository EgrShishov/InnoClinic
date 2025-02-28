using Microsoft.AspNetCore.Http;

public class UploadDocumentRequest
{
    public IFormFile File { get; init; }
    public int ResultId { get; init; }
}
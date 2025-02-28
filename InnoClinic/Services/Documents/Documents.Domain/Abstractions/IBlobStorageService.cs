public interface IBlobStorageService
{
    public Task<string> UploadAsync(Stream stream, string fileName, string containerName, string contentType, CancellationToken cancellationToken = default);
    public Task<FileResponse> DownloadAsync(string fileName, string containerName, CancellationToken cancellationToken = default);
    public Task<FileResponse> DownloadAsync(string blobUri, CancellationToken cancellationToken = default);
    public Task<bool> DeleteAsync(string fileName, string containerName, CancellationToken cancellationToken = default);
    public Task<bool> DeleteAsync(string blobUri, CancellationToken cancellationToken = default);
    public Task<bool> ExistsAsync(string blobUri, CancellationToken cancellationToken = default);
}

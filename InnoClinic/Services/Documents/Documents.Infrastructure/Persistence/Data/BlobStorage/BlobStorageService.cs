using Azure.Storage.Blobs.Models;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobStorageService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;

        BlobContainerClient photosContainer = _blobServiceClient.GetBlobContainerClient("photos");
        if (!photosContainer.Exists())
        {
            photosContainer.Create();
            photosContainer.SetAccessPolicy(PublicAccessType.Blob);
        }

        BlobContainerClient documentsContainer = _blobServiceClient.GetBlobContainerClient("documents");
        if (!documentsContainer.Exists())
        {
            documentsContainer.Create();
            documentsContainer.SetAccessPolicy(PublicAccessType.Blob);
        }
    }

    public async Task<string> UploadAsync(Stream stream, string fileName, string containerName, string contentType, CancellationToken cancellationToken = default)
    {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);

        BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);

        await blobClient.UploadAsync(stream,
              new BlobUploadOptions
              {
                  HttpHeaders = new BlobHttpHeaders
                  {
                      ContentType = contentType
                  }
              },
              cancellationToken);

        return blobClient.Uri.ToString();
    }

    public async Task<FileResponse> DownloadAsync(string fileName, string containerName, CancellationToken cancellationToken = default)
    {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);

        BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);

        var downloadResult = await blobClient.DownloadContentAsync(cancellationToken: cancellationToken);

        return new FileResponse
        {
            Content = downloadResult.Value.Content.ToStream(),
            ContentType = downloadResult.Value.Details.ContentType,
            Filename = blobClient.Name
        };
    } 
    public async Task<FileResponse> DownloadAsync(string blobUri, CancellationToken cancellationToken = default)
    {
        BlobUriBuilder blobUriBuilder = new BlobUriBuilder(new Uri(blobUri));

        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(blobUriBuilder.BlobContainerName);

        BlobClient blobClient = blobContainerClient.GetBlobClient(blobUriBuilder.BlobName);

        var downloadResult = await blobClient.DownloadContentAsync(cancellationToken: cancellationToken);

        return new FileResponse
        {
            Content = downloadResult.Value.Content.ToStream(),
            ContentType = downloadResult.Value.Details.ContentType,
            Filename = blobClient.Name
        };
    }

    public async Task<bool> DeleteAsync(string fileName, string containerName, CancellationToken cancellationToken = default)
    {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);

        BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);

        BlobProperties blobProperties = await blobClient.GetPropertiesAsync();
        if (blobProperties.LeaseState != LeaseState.Available)
        {
            return false;
        }

        var response = await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);

        return response.Value;
    }
    public async Task<bool> DeleteAsync(string blobUri, CancellationToken cancellationToken = default)
    {
        BlobUriBuilder blobUriBuilder = new BlobUriBuilder(new Uri(blobUri));

        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(blobUriBuilder.BlobContainerName);

        BlobClient blobClient = blobContainerClient.GetBlobClient(blobUriBuilder.BlobName);

        BlobProperties blobProperties = await blobClient.GetPropertiesAsync();
        if (blobProperties.LeaseState != LeaseState.Available)
        {
            return false;
        }

        var response = await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);

        return response.Value;
    }
    public async Task<bool> ExistsAsync(string blobUri, CancellationToken cancellationToken = default)
    {
        BlobUriBuilder blobUriBuilder = new BlobUriBuilder(new Uri(blobUri));

        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(blobUriBuilder.BlobContainerName);

        var blobClient = blobContainerClient.GetBlobClient(blobUriBuilder.BlobName);

        return await blobClient.ExistsAsync(cancellationToken);
    }
}

using Microsoft.AspNetCore.Http;

public class FileFacade : IFileFacade
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobStorageService _blobStorageService;
    private readonly string DocumentsContainerName = "documents";
    private readonly string PhotosContainerName = "photos";
    public FileFacade(IUnitOfWork unitOfWork, IBlobStorageService blobStorageService)
    {
        _unitOfWork = unitOfWork;
        _blobStorageService = blobStorageService;
    }

    public async Task<ErrorOr<Unit>> DeleteResultDocumentAsync(int resultId)
    {
        try
        {
            var document = await _unitOfWork.Documents.GetDocumentByResultAsync(resultId);
            
            if (document is null)
            {
                return Errors.Documents.NotFound;
            }

            if (!await _blobStorageService.ExistsAsync(document.Url))
            {
                return Errors.Documents.BlobNotFound;
            }

            await _blobStorageService.DeleteAsync(document.Url);
            await _unitOfWork.Documents.DeleteDocumentAsync(document.Id);
            await _unitOfWork.CompleteAsync();
        }
        catch (Exception ex)
        {
            return Error.Failure(code: "File.UnhandledException", description: ex.Message);
        }

        return Unit.Value;
    }

    public async Task<ErrorOr<Unit>> DeletePhotoAsync(string url)
    {
        try
        {
            var photo = await _unitOfWork.Photos.GetPhotoAsync(url);

            if (photo is null)
            {
                return Errors.Photos.NotFound;
            }
            
            if (!await _blobStorageService.ExistsAsync(photo.Url))
            {
                return Errors.Photos.BlobNotFound;
            }

            await _blobStorageService.DeleteAsync(photo.Url);
            await _unitOfWork.Photos.DeletePhotoAsync(photo.Url);
            await _unitOfWork.CompleteAsync();
        }
        catch (Exception ex)
        {
            return Error.Failure(code: "File.UnhandledException", description: ex.Message);
        }

        return Unit.Value;
    }

    public async Task<ErrorOr<FileResponse>> DownloadDocumentAsync(int resultId)
    {
        var document = await _unitOfWork.Documents.GetDocumentByResultAsync(resultId);

        if (document is null)
        {
            return Errors.Documents.NotFoundWithResultsId(resultId);
        }

        var blobFileResponse = await _blobStorageService.DownloadAsync(document.Url);

        if (blobFileResponse is null)
        {
            return Errors.Documents.BlobNotFound;
        }

        return blobFileResponse;
    }

    public async Task<ErrorOr<FileResponse>> DownloadPhotoAsync(string url)
    {
        var photo = await _unitOfWork.Photos.GetPhotoAsync(url);

        if (photo is null)
        {
            return Errors.Photos.NotFound;
        }

        var blobFileResponse = await _blobStorageService.DownloadAsync(url);

        if (blobFileResponse is null)
        {
            return Errors.Photos.BlobNotFound;
        }

        return blobFileResponse;
    }

    public async Task<ErrorOr<string>> UploadDocumentAsync(IFormFile file, int resultId)
    {
        if (file == null || file.Length == 0)
        {
            return Errors.Documents.InvalidFile;
        }

        var url = await _blobStorageService
            .UploadAsync(file.OpenReadStream(), file.FileName, DocumentsContainerName, file.ContentType);

        var document = new Document
        {
            Url = url,
            ResultId = resultId
        };

        await _unitOfWork.Documents.SaveDocumentAsync(document);

        await _unitOfWork.CompleteAsync();

        return url;
    }

    public async Task<ErrorOr<string>> UploadPhotoAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return Errors.Photos.NotFound;
        }

        var url = await _blobStorageService
            .UploadAsync(file.OpenReadStream(), file.FileName, PhotosContainerName, file.ContentType);

        var photo = new Photo
        {
            Url = url
        };

        await _unitOfWork.Photos.SavePhotoAsync(photo);

        await _unitOfWork.CompleteAsync();

        return url;
    }
}

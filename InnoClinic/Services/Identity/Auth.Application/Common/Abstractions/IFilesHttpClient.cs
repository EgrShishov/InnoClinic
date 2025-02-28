public interface IFilesHttpClient
{
    public Task<ErrorOr<string>> UploadDocument(UploadDocumentRequest request);
    public Task<ErrorOr<FileResponse>> DownloadDocument(string fileName);
    public Task<ErrorOr<string>> UploadPhoto(UploadPhotoRequest request);
    public Task<ErrorOr<Unit>> DeletePhoto(string fileName);
    public Task<ErrorOr<Unit>> DeleteDocument(string fileName);
}
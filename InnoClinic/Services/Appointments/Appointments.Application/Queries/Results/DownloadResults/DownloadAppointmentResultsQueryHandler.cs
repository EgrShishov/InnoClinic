using Microsoft.AspNetCore.Http;

public class DownloadAppointmentResultsQueryHandler(
    IUnitOfWork unitOfWork, 
    IFilesHttpClient filesHttpClient) : IRequestHandler<DownloadAppointmentResultsQuery, ErrorOr<IFormFile>>
{
    public async Task<ErrorOr<IFormFile>> Handle(DownloadAppointmentResultsQuery request, CancellationToken cancellationToken)
    {
        var results = await unitOfWork.ResultsRepository.GetResultsByIdAsync(request.ResultsId);

        if (results is null)
        {
            return Errors.Results.NotFound;
        }

        var pdfResultsResponse = await filesHttpClient.GetDocumentForResultAsync(request.ResultsId);

        if (pdfResultsResponse.IsError)
        {
            return pdfResultsResponse.FirstError;
        }

        var formFile = pdfResultsResponse.Value;

        using var memoryStream = new MemoryStream();
        await formFile.OpenReadStream().CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        var updatedFormFile = new FormFile(memoryStream, 0, memoryStream.Length, formFile.Name, formFile.FileName)
        {
            Headers = formFile.Headers,
            ContentType = formFile.ContentType,
            ContentDisposition = formFile.ContentDisposition
        };

        return updatedFormFile;
    }
}

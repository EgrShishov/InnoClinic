using Microsoft.AspNetCore.Http;

public sealed record DownloadAppointmentResultsQuery(int ResultsId) : IRequest<ErrorOr<IFormFile>>
{
}
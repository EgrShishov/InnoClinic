using Microsoft.AspNetCore.Http;

public interface IPDFDocumentGenerator
{
    public IFormFile GenerateAppointmentResults(GeneratePDFResultsRequest results);
}

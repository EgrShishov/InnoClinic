using Microsoft.AspNetCore.Mvc;

public class Files : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/documents/by-result/{resultId}", async (IFileFacade fileFacade, int resultId) =>
        {
            var response = await fileFacade.DownloadDocumentAsync(resultId);

            return response.Match(
                response =>
                {
                    using var memoryStream = new MemoryStream();
                    response.Content.CopyTo(memoryStream);
                    memoryStream.Position = 0;
                    return Results.File(memoryStream, response.ContentType, response.Filename);
                },
                errors => Results.Extensions.Problem(errors));
        })
        .WithName("DocumentsAccordingResults")
        .DisableAntiforgery();
        //.RequireAuthorization("Patient, Doctor, Receptionist");

        app.MapPost("/api/document", async (IFileFacade fileFacade, [FromForm] IFormFile File, [FromForm] int ResultId) =>
        {
            var response = await fileFacade
                .UploadDocumentAsync(File, ResultId);

            return response.Match(
                url => Results.Ok(url),
                errors => Results.Extensions.Problem(errors));
        })
        .WithName("UploadDocument")
        .DisableAntiforgery();
        //.RequireAuthorization("Patient, Doctor, Receptionist");

        app.MapGet("/api/document/{resultId}", async (IFileFacade fileFacade, int resultId) =>
        {
            var response = await fileFacade.DownloadDocumentAsync(resultId);

            return response.Match(
                response => Results.File(response.Content, response.ContentType, response.Filename),
                errors => Results.Extensions.Problem(errors));
        })
        .WithName("DownloadDocument")
        .DisableAntiforgery();
        //.RequireAuthorization("Patient, Doctor, Receptionist");

        app.MapDelete("/api/document/{resultId}", async (IFileFacade fileFacade, int resultId) =>
        {
            var response = await fileFacade.DeleteResultDocumentAsync(resultId);

            return response.Match(
                _ => Results.NoContent(),
                errors => Results.Extensions.Problem(errors));
        })
        .WithName("DeleteDocument")
        .DisableAntiforgery();
        //.RequireAuthorization("Patient, Doctor, Receptionist");

        app.MapPost("api/photos", async (IFileFacade fileFacade, IFormFile photo) =>
        {
            var response = await fileFacade
                .UploadPhotoAsync(photo);

            return response.Match(
                url => Results.Ok(url),
                errors => Results.Extensions.Problem(errors));
        })
        .WithName("UploadPhoto")
        .DisableAntiforgery();
        //.RequireAuthorization("Patient, Doctor, Receptionist");

        app.MapDelete("/api/photo/{fileName}", async (IFileFacade fileFacade, string filename) =>
        {
            var response = await fileFacade.DeletePhotoAsync(filename);

            return response.Match(
                _ => Results.NoContent(),
                errors => Results.Extensions.Problem(errors));
        })
        .WithName("DeletePhoto")
        .DisableAntiforgery();
        //.RequireAuthorization("Patient, Doctor, Receptionist");
    }
}
    

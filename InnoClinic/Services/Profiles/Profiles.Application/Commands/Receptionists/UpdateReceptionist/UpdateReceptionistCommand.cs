using Microsoft.AspNetCore.Http;

public sealed record UpdateReceptionistCommand(
    int ReceptionistId,
    string FirstName,
    string LastName,
    string MiddleName,
    string Email,
    IFormFile Photo,
    string OfficeId) : IRequest<ErrorOr<Receptionist>>
{
}

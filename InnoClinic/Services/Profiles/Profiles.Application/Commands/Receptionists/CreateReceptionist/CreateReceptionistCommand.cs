using Microsoft.AspNetCore.Http;

public sealed record CreateReceptionistCommand(
    string FirstName,
    string LastName,
    string MiddleName,
    string Email,
    string PhoneNumber,
    int CreatedBy,
    string OfficeId,
    IFormFile Photo) : IRequest<ErrorOr<CreateReceptionistProfileResponse>>
{
}

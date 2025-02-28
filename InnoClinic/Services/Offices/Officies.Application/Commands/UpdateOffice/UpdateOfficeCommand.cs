using Microsoft.AspNetCore.Http;

public sealed record UpdateOfficeCommand(
    string OfficeId,
    string City,
    string Street,
    string HouseNumber,
    string OfficeNumber,
    IFormFile? Photo,
    string RegistryPhoneNumber,
    bool IsActive) : IRequest<ErrorOr<Unit>>
{
}

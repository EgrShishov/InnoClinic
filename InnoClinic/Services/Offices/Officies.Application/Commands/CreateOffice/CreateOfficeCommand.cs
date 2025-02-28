using Microsoft.AspNetCore.Http;

public sealed record CreateOfficeCommand(
    string City,
    string Street,
    string HouseNumber,
    string OfficeNumber,
    IFormFile Photo,
    string RegistryPhoneNumber,
    bool IsActive) : IRequest<ErrorOr<OfficeResponse>>
{
}

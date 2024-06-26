
namespace InnoClinic.Contracts.Offices.Responses
{
    public record OfficeResponse(string Id,
        string City,
        string Street,
        string HouseNumber,
        string OfficeNumber,
        string PhotoId,
        string RegistryPhoneNumber,
        bool IsActive);
}

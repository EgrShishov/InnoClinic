
namespace InnoClinic.Contracts.Offices.Requests
{
    public record UpdateOfficeRequest(string City,
          string Street,
          string HouseNumber,
          string OfficeNumber,
          string PhotoId,
          string RegistryPhoneNumber,
          bool IsActive);
}

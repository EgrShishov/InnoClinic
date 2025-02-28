using Microsoft.AspNetCore.Http;

public class CreateOfficeRequest
{
    public string City { get; init; }
    public string Street {  get; init; }
    public string HouseNumber { get; init; }
    public string OfficeNumber {  get; init; }
    public IFormFile Photo { get; init; }
    public string RegistryPhoneNumber { get; init; }
    public bool IsActive { get; init; }
}

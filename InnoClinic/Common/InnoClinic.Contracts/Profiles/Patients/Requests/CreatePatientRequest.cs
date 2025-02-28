using Microsoft.AspNetCore.Http;

public class CreatePatientRequest
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string MiddleName { get; init; }
    public string PhoneNumber { get; init; }
    public string Email { get; init; }
    public DateTime DateOfBirth { get; init; }
    public IFormFile Photo {  get; init; }
}

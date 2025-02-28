using Microsoft.AspNetCore.Http;

public class CreateReceptionistRequest
{
    public IFormFile Photo { get; init; }
    public string FirstName {  get; init; }
    public string LastName {  get; init; }
    public string MiddleName {  get; init; }
    public string Email { get; init; }
    public string OfficeId {  get; init; }
    public string PhoneNumber {  get; init; }
}
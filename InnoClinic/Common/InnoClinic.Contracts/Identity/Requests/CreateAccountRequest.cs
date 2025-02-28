using Microsoft.AspNetCore.Http;

public class CreateAccountRequest
{
    public IFormFile Photo { get; set; }
    public string Email {  get; set; }
    public string? PhoneNumber {  get; set; }
    public string Role { get; set; }
    public int CreatedBy { get; set; }
}
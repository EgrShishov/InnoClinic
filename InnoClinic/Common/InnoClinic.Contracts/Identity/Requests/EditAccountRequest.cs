using Microsoft.AspNetCore.Http;

public class EditAccountRequest
{
    public int UpdatedBy { get; init; }
    public string PhoneNumber { get; init; }
    public IFormFile Photo { get; init; }
}

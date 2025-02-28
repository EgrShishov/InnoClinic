using Microsoft.AspNetCore.Identity;

public class Account : IdentityUser<int>
{
    public string PhotoUrl { get; set; } = string.Empty;
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
}

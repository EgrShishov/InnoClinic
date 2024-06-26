
namespace InnoClinic.Contracts.Authentication.Requests
{
    public record SignInRequest(string email, string password, string reentered_password);
}

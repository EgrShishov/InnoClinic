
namespace InnoClinic.Contracts.Authentication.Requests
{
    public record SignUpRequest(string email, string password, string reentered_password);

}

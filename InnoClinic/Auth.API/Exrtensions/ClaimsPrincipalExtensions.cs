using System.Security.Claims;

namespace Auth.API.Exrtensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return int.TryParse(user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value, out int id) ? id : 0;
        }

    }
}

using System.Security.Claims;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        return int.TryParse(user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value, out int id) ? id : 0;
    }

    public static string GetRole(this ClaimsPrincipal user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        return user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
    }
}

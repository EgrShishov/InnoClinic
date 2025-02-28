using Microsoft.AspNetCore.Identity;

public static class RoleSeeder
{
    public static async Task SeedRolesAsync(RoleManager<AppRole> roleManager)
    {
        var roles = new[] { "Doctor", "Patient", "Receptionist" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new AppRole { Name = role });
            }
        }
    }
}
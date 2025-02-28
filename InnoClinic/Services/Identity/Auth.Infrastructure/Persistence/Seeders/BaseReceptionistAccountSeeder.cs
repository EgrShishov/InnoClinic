using Microsoft.AspNetCore.Identity;

public class BaseReceptionistAccountSeeder
{
    public static async Task SeedBaseReceptionistAccountAsync(
        RoleManager<AppRole> roleManager,
        UserManager<Account> userManager)
    {
        string roleName = "Receptionist";

        if (!await roleManager.RoleExistsAsync(roleName))
        {
            var role = new AppRole { Name = roleName };
            var roleResult = await roleManager.CreateAsync(role);

            if (!roleResult.Succeeded)
            {
                throw new Exception("Failed to create Receptionist role");
            }
        }

        const string receptionistEmail = "receptionist@innoclinic.com";
        const string defaultPassword = "SecurePassword123!";
        string defaultPhoneNumber = string.Empty;

        var receptionistUser = await userManager.FindByEmailAsync(receptionistEmail);

        if (receptionistUser == null)
        {
            receptionistUser = new Account
            {
                UserName = receptionistEmail,
                Email = receptionistEmail,
                EmailConfirmed = true,
                PhoneNumber = defaultPhoneNumber,
                PhoneNumberConfirmed = true,
            };

            var userCreationResult = await userManager.CreateAsync(receptionistUser, defaultPassword);

            if (!userCreationResult.Succeeded)
            {
                throw new Exception("Failed to create receptionist user account");
            }

            var roleAssignmentResult = await userManager.AddToRoleAsync(receptionistUser, roleName);

            if (!roleAssignmentResult.Succeeded)
            {
                throw new Exception("Failed to assign Receptionist role to the user");
            }
        }
    }
}
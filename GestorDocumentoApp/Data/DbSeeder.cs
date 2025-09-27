using Microsoft.AspNetCore.Identity;

namespace GestorDocumentoApp.Data
{
    public class DbSeeder
    {
        public static async Task SeedAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,IConfiguration configuration)
        {
            var roles = new[] {"Admin"};
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var adminDefaul=await userManager.FindByEmailAsync(configuration["AdminUser:Email"]);

            if (adminDefaul == null) {
                var admin = new IdentityUser
                {
                    UserName = configuration["AdminUser:Email"],
                    Email = configuration["AdminUser:Email"]
                };
                var result = await userManager.CreateAsync(admin, configuration["AdminUser:Password"]);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}

using BlazorEcommerce.Constants;
using Microsoft.AspNetCore.Identity;

namespace BlazorEcommerce.Data
{
    public static class DataSeeder
    {
        public static async Task SeedDefaultAdminUserAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }

            var defaultAdminUser = await userManager.FindByEmailAsync("admin@yay.com");
            if (defaultAdminUser == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@yay.com",
                    Email = "admin@yay.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, UserRoles.Admin);
                }
            }
        }
    }
}
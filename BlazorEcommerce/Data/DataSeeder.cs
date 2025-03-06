using BlazorEcommerce.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

        public static async Task SeedSampleDataAsync(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            if (!dbContext.ProductCategory.Any())
            {
                var categories = new List<ProductCategory>
                {
                    new ProductCategory { Name = "Tea" },
                    new ProductCategory { Name = "Cafe" },
                    new ProductCategory { Name = "Freeze" },
                    new ProductCategory { Name = "Others" }
                };

                dbContext.ProductCategory.AddRange(categories);
                await dbContext.SaveChangesAsync();
            }

            if (!dbContext.Product.Any()) 
            {
                var teaCategory = await dbContext.ProductCategory.SingleAsync(c => c.Name == "Tea");
                var teaProducts = new List<Product>();

                for (int i = 1; i <= 12; i++)
                {
                    teaProducts.Add(new Product
                    {
                        Name = $"Tea {i}",
                        Description = $"Tea {i} Description",
                        Price = 40000 + (i * 1000),
                        ImageUrl = $"/images/products/Tea{i}.jpg",
                        CategoryId = teaCategory!.Id
                    });
                }
                dbContext.Product.AddRange(teaProducts);

                var cafeCategory = await dbContext.ProductCategory.SingleAsync(c => c.Name == "Cafe");
                var cafeProducts = new List<Product>();

                for (int i = 1; i <= 6; i++)
                {
                    cafeProducts.Add(new Product
                    {
                        Name = $"Cafe {i}",
                        Description = $"Cafe {i} Description",
                        Price = 50000 + (i * 1000),
                        ImageUrl = $"/images/products/Cafe{i}.jpg",
                        CategoryId = cafeCategory.Id
                    });
                }
                dbContext.Product.AddRange(cafeProducts);

                var freezeCategory = await dbContext.ProductCategory.SingleAsync(c => c.Name == "Freeze");
                var freezeProducts = new List<Product>();
                for (int i = 1; i <= 3; i++)
                {
                    freezeProducts.Add(new Product
                    {
                        Name = $"Freeze {i}",
                        Description = $"Freeze {i} Description",
                        Price = 60000 + (i * 1000),
                        ImageUrl = $"/images/products/Freeze{i}.jpg",
                        CategoryId = freezeCategory.Id
                    });
                }
                dbContext.Product.AddRange(freezeProducts);

                var othersCategory = await dbContext.ProductCategory.SingleAsync(c => c.Name == "Others");
                var otherProducts = new List<Product>();
                for (int i = 1; i <= 3; i++)
                {
                    otherProducts.Add(new Product
                    {
                        Name = $"Pastry {i}",
                        Description = $"Pastry {i} Description",
                        Price = 90000 + (i * 1000),
                        ImageUrl = $"/images/products/Others{i}.jpg",
                        CategoryId = othersCategory.Id
                    });
                }
                dbContext.Product.AddRange(otherProducts);

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
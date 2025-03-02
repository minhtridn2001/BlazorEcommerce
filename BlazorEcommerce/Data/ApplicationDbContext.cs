using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BlazorEcommerce.Data;

namespace BlazorEcommerce.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<BlazorEcommerce.Data.ProductCategory> ProductCategory { get; set; } = default!;
        public DbSet<BlazorEcommerce.Data.Product> Product { get; set; } = default!;
        public DbSet<BlazorEcommerce.Data.CartItem> CartItem { get; set; } = default!;
    }
}

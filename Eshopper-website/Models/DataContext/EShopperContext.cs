using Eshopper_website.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Eshopper_website.Models.DataContext
{
    public class EShopperContext : IdentityDbContext<AppUser>
    {
        public EShopperContext(DbContextOptions<EShopperContext> options) : base(options)
        {
        }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
    }
}

using Eshopper_website.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Eshopper_website.Models;

namespace Eshopper_website.Models.DataContext
{
    public class EShopperContext : DbContext
    {
        public EShopperContext(DbContextOptions<EShopperContext> options) : base(options)
        {
        }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Contact> Contacts { get; set; }
    }
}

using Eshopper_website.Models.GHN;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<ProductQuantity> ProductQuantities { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<AccountRole> AccountRole { get; set; }
        public DbSet<AccountStatusLogin> AccountStatusLogins { get; set; }
        public DbSet<Shipping> Shippings { get; set; }
        public DbSet<AccountLogin> AccountLogins { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Compare> Compares { get; set; }
        public DbSet<MomoInfo> Momos { get; set; }
        public DbSet<Statistical> Statisticals { get; set; }
		public DbSet<Payment> Payments { get; set; }
		public DbSet<OrderGHN> OrderGHNs { get; set; }
	}
}

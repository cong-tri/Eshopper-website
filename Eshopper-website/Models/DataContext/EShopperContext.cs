﻿using Eshopper_website.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Eshopper_website.Models;

namespace Eshopper_website.Models.DataContext
{
    public class EShopperContext : IdentityDbContext<AppUser>
    {
        public EShopperContext(DbContextOptions<EShopperContext> options) : base(options)
        {
        }

        public DbSet<Brand>? Brands { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Product>? Products { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Blog>? Blogs { get; set; }
        public DbSet<Coupon>? Coupons { get; set; }
        public DbSet<Menu>? Menus { get; set; }


    }
}
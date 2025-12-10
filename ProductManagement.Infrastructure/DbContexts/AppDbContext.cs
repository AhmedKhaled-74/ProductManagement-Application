using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Infrastructure.DbContexts
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<SubCategory> SubCategories { get; set; }
        public virtual DbSet<CartProduct> CartProducts { get; set; }
        public virtual DbSet<ProductCustomAttribute> ProductCustomAttributes { get; set; }
        public virtual DbSet<ProductMedia> ProductMedias { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<WhishList> WhishLists { get; set; }
        public virtual DbSet<WhishListProduct> WhishListProducts { get; set; }
        public virtual DbSet<Region> Regions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureProductManagement();
            base.OnModelCreating(modelBuilder);
        }
    }
}

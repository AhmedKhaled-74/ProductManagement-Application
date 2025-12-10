using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Entities;

namespace ProductManagement.Infrastructure.DbContexts
{
    public class AppReadDbContext(DbContextOptions<AppReadDbContext> options)
        : DbContext(options)
    {
        public virtual DbSet<Product> Products { get; set; }
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
        public virtual DbSet<Vendor> Vendors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureProductManagement();
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            throw new InvalidOperationException("This DbContext is read-only.");
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            throw new InvalidOperationException("This DbContext is read-only.");
        }
    }
}

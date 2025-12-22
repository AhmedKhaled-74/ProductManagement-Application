using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Entities;

namespace ProductManagement.Infrastructure.DbContexts
{
    public static class ModelBuilderExtensions
    {
        public static void ConfigureProductManagement(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.ProductId);

                // Configure relationships
                entity.HasMany(p => p.Reviews)
                    .WithOne(r => r.Product)
                    .HasForeignKey(r => r.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.ProductMedias)
                    .WithOne(pm => pm.Product)
                    .HasForeignKey(pm => pm.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.ProductCustomAttributes)
                    .WithOne(pca => pca.Product)
                    .HasForeignKey(pca => pca.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.WhishListProducts)
                    .WithOne(wlp => wlp.Product)
                    .HasForeignKey(wlp => wlp.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.CartProducts)
                    .WithOne(cp => cp.Product)
                    .HasForeignKey(cp => cp.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Configure navigation properties
                entity.HasOne(p => p.ProductCategory)
                    .WithMany(c => c.Products)
                    .HasForeignKey(p => p.ProductCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Vendor)
                    .WithMany(c => c.Products)
                    .HasForeignKey(p => p.VendorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.ProductSubCategory)
                    .WithMany(sc => sc.Products)
                    .HasForeignKey(p => p.ProductSubCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.ProductBrand)
                    .WithMany(b => b.Products)
                    .HasForeignKey(p => p.BrandId)
                    .OnDelete(DeleteBehavior.SetNull);

                // Configure properties
                entity.Property(p => p.ProductName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(p => p.ProductDescription)
                    .IsRequired()
                    .HasMaxLength(1000);


                entity.Property(p => p.ProductMass)
                    .HasColumnType("decimal(10,2)");

                entity.Property(p => p.ProductVolume)
                    .HasColumnType("decimal(10,2)");

                entity.Property(p => p.ProductPrice)
                    .HasColumnType("decimal(18,2)");

                entity.Property(p => p.Discount)
                    .HasColumnType("decimal(5,2)")
                    .HasDefaultValue(0);

                entity.Property(p => p.NumberOfRemainingProducts)
                    .HasDefaultValue(0);

                entity.Property(p => p.SoldTimes)
                    .HasDefaultValue(0);

                entity.Property(p => p.TotalRatedUsers)
                    .HasDefaultValue(0);

                entity.Property(p => p.TotalStars)
                    .HasDefaultValue(0);

                entity.HasQueryFilter(p => p.IsApproved);

            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(c => c.CartId);

                entity.Property(c => c.UserId)
                .IsRequired();
                entity.HasIndex(c => c.UserId)
                .IsUnique();

                entity.HasMany(c => c.CartProducts)
                .WithOne(cp => cp.Cart)
                .HasForeignKey(cp => cp.CartId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CartProduct>(entity =>
            {
                entity.HasKey(cp => new { cp.CartId, cp.ProductId }); // composite PK

                entity.Property(cp => cp.Quantity)
                .IsRequired();
            });

            modelBuilder.Entity<WhishList>(entity =>
            {
                entity.HasKey(w => w.WhishListId);

                entity.Property(c => c.UserId)
                .IsRequired();
                entity.HasIndex(c => c.UserId)
                .IsUnique();

                entity.HasMany(w => w.WhishListProducts)
                .WithOne(wp => wp.WhishList)
                .HasForeignKey(wp => wp.WhishListId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<WhishListProduct>(entity =>
            {
                entity.HasKey(wp => new { wp.WhishListId, wp.ProductId }); // composite PK

            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.Property(b => b.BrandName).HasMaxLength(100)
                .IsRequired();

                entity.HasIndex(b => b.BrandName)
                .IsUnique();

            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.CategoryId);

                entity.HasMany(c => c.SubCategories)
                .WithOne(c => c.Category)
                .HasForeignKey(c => c.CategoryId);
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.HasKey(r => r.RegionId);

                entity.Property(r => r.RegionName).HasMaxLength(100);
                entity.Property(r => r.ConstTax).HasColumnType("decimal(5,3)");
                entity.Property(r => r.PoeRegion).HasColumnType("decimal(5,3)");
            });

        }
    }
}
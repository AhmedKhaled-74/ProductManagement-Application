using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagement.Infrastructure.DbContexts
{
    public static class SeedData
    {
        public static async Task Initialize(AppDbContext context)
        {
            // Check if data already exists
            if (await context.Categories.AnyAsync())
            {
                return; // Database has been seeded
            }

            await SeedBrands(context);
            await SeedVendors(context);
            await SeedCategoriesAndSubCategories(context);
            await SeedProducts(context);
            await SeedProductMedia(context);
            await SeedProductCustomAttributes(context);
            await SeedReviews(context);
            await SeedRegions(context);
            await SeedCartsAndWishlists(context);
        }

        private static async Task SeedBrands(AppDbContext context)
        {
            var brands = new List<Brand>
            {
                new Brand { BrandId = Guid.NewGuid(), BrandName = "Nike" },
                new Brand { BrandId = Guid.NewGuid(), BrandName = "Adidas" },
                new Brand { BrandId = Guid.NewGuid(), BrandName = "Apple" },
                new Brand { BrandId = Guid.NewGuid(), BrandName = "Samsung" },
                new Brand { BrandId = Guid.NewGuid(), BrandName = "Sony" },
                new Brand { BrandId = Guid.NewGuid(), BrandName = "Dell" }
            };

            await context.Brands.AddRangeAsync(brands);
            await context.SaveChangesAsync();
        }

        private static async Task SeedVendors(AppDbContext context)
        {
            var vendors = new List<Vendor>
            {
                new Vendor { VendorId = Guid.NewGuid(), VendorName = "Nike Store" },
                new Vendor { VendorId = Guid.NewGuid(), VendorName = "Apple Store" },
                new Vendor { VendorId = Guid.NewGuid(), VendorName = "Sony Store" },
                new Vendor { VendorId = Guid.NewGuid(), VendorName = "Adidas Store" },
            };

            await context.Vendors.AddRangeAsync(vendors);
            await context.SaveChangesAsync();
        }

        private static async Task SeedCategoriesAndSubCategories(AppDbContext context)
        {
            var categories = new List<Category>
            {
                new Category { CategoryId = Guid.NewGuid(), CategoryName = "Electronics" },
                new Category { CategoryId = Guid.NewGuid(), CategoryName = "Clothing" },
                new Category { CategoryId = Guid.NewGuid(), CategoryName = "Sports" },
                new Category { CategoryId = Guid.NewGuid(), CategoryName = "Home & Garden" }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            var subCategories = new List<SubCategory>
            {
                // Electronics subcategories
                new SubCategory {
                    SubCategoryId = Guid.NewGuid(),
                    SubCategoryName = "Smartphones",
                    CategoryId = categories[0].CategoryId
                },
                new SubCategory {
                    SubCategoryId = Guid.NewGuid(),
                    SubCategoryName = "Laptops",
                    CategoryId = categories[0].CategoryId
                },
                new SubCategory {
                    SubCategoryId = Guid.NewGuid(),
                    SubCategoryName = "Headphones",
                    CategoryId = categories[0].CategoryId
                },
                
                // Clothing subcategories
                new SubCategory {
                    SubCategoryId = Guid.NewGuid(),
                    SubCategoryName = "Shoes",
                    CategoryId = categories[1].CategoryId
                },
                new SubCategory {
                    SubCategoryId = Guid.NewGuid(),
                    SubCategoryName = "T-Shirts",
                    CategoryId = categories[1].CategoryId
                },
                
                // Sports subcategories
                new SubCategory {
                    SubCategoryId = Guid.NewGuid(),
                    SubCategoryName = "Fitness Equipment",
                    CategoryId = categories[2].CategoryId
                }
            };

            await context.SubCategories.AddRangeAsync(subCategories);
            await context.SaveChangesAsync();
        }

        private static async Task SeedProducts(AppDbContext context)
        {
            var brands = await context.Brands.ToListAsync();
            var vendors = await context.Vendors.ToListAsync();
            var categories = await context.Categories.ToListAsync();
            var subCategories = await context.SubCategories.ToListAsync();

            var products = new List<Product>
            {
                new Product
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "iPhone 15 Pro",
                    ProductDescription = "Latest iPhone with advanced camera system",
                    ProductMass = 0.187m,
                    ProductVolume = 0.1m,
                    ProductPrice = 999.99m,
                    VendorId = vendors.First(b => b.VendorName == "Apple Store").VendorId,
                    BrandId = brands.First(b => b.BrandName == "Apple").BrandId,
                    ProductCategoryId = categories.First(c => c.CategoryName == "Electronics").CategoryId,
                    ProductSubCategoryId = subCategories.First(sc => sc.SubCategoryName == "Smartphones").SubCategoryId,
                    NumberOfRemainingProducts = 50,
                    SoldTimes = 150,
                    Discount = 0.1m, // 10% discount
                    TotalRatedUsers = 45,
                    TotalStars = 215,
                    IsApproved = true
                },
                new Product
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "MacBook Pro 16-inch",
                    ProductDescription = "Powerful laptop for professionals",
                    ProductMass = 2.1m,
                    ProductVolume = 1.5m,
                    ProductPrice = 2399.99m,
                    VendorId = vendors.First(b => b.VendorName == "Apple Store").VendorId,
                    BrandId = brands.First(b => b.BrandName == "Apple").BrandId,
                    ProductCategoryId = categories.First(c => c.CategoryName == "Electronics").CategoryId,
                    ProductSubCategoryId = subCategories.First(sc => sc.SubCategoryName == "Laptops").SubCategoryId,
                    NumberOfRemainingProducts = 25,
                    SoldTimes = 80,
                    Discount = 0.05m, // 5% discount
                    TotalRatedUsers = 32,
                    TotalStars = 152,
                    IsApproved = true
                },
                new Product
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Nike Air Max 270",
                    ProductDescription = "Comfortable running shoes with air cushioning",
                    ProductMass = 0.3m,
                    ProductVolume = 0.4m,
                    ProductPrice = 150.00m,
                    VendorId = vendors.First(b => b.VendorName == "Nike Store").VendorId,
                    BrandId = brands.First(b => b.BrandName == "Nike").BrandId,
                    ProductCategoryId = categories.First(c => c.CategoryName == "Clothing").CategoryId,
                    ProductSubCategoryId = subCategories.First(sc => sc.SubCategoryName == "Shoes").SubCategoryId,
                    NumberOfRemainingProducts = 100,
                    SoldTimes = 300,
                    Discount = 0.15m, // 15% discount
                    TotalRatedUsers = 67,
                    TotalStars = 321,
                    IsApproved = true
                },
                new Product
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Sony WH-1000XM4",
                    ProductDescription = "Wireless noise-canceling headphones",
                    ProductMass = 0.254m,
                    ProductVolume = 0.3m,
                    ProductPrice = 349.99m,
                    VendorId = vendors.First(b => b.VendorName == "Sony Store").VendorId,
                    BrandId = brands.First(b => b.BrandName == "Sony").BrandId,
                    ProductCategoryId = categories.First(c => c.CategoryName == "Electronics").CategoryId,
                    ProductSubCategoryId = subCategories.First(sc => sc.SubCategoryName == "Headphones").SubCategoryId,
                    NumberOfRemainingProducts = 75,
                    SoldTimes = 200,
                    Discount = 0.0m,
                    TotalRatedUsers = 89,
                    TotalStars = 425,
                    IsApproved = true

                },
                new Product
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Adidas Ultraboost 21",
                    ProductDescription = "High-performance running shoes",
                    ProductMass = 0.32m,
                    ProductVolume = 0.45m,
                    VendorId = vendors.First(b => b.VendorName == "Adidas Store").VendorId,
                    BrandId = brands.First(b => b.BrandName == "Adidas").BrandId,
                    ProductCategoryId = categories.First(c => c.CategoryName == "Clothing").CategoryId,
                    ProductSubCategoryId = subCategories.First(sc => sc.SubCategoryName == "Shoes").SubCategoryId,
                    NumberOfRemainingProducts = 60,
                    SoldTimes = 180,
                    Discount = 0.2m, // 20% discount
                    TotalRatedUsers = 54,
                    TotalStars = 256,
                    IsApproved = true
                }
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }

        private static async Task SeedProductMedia(AppDbContext context)
        {
            var products = await context.Products.ToListAsync();
            var productMedia = new List<ProductMedia>();

            foreach (var product in products)
            {
                productMedia.AddRange(new[]
                {
                    new ProductMedia
                    {
                        ProductMediaId = Guid.NewGuid(),
                        Type = "image",
                        ProductId = product.ProductId,
                        ProductMediaURL = $"https://example.com/images/{product.ProductId}/main.jpg"
                    },
                    new ProductMedia
                    {
                        ProductMediaId = Guid.NewGuid(),
                        Type = "image",
                        ProductId = product.ProductId,
                        ProductMediaURL = $"https://example.com/images/{product.ProductId}/angle1.jpg"
                    },
                    new ProductMedia
                    {
                        ProductMediaId = Guid.NewGuid(),
                        Type = "video",
                        ProductId = product.ProductId,
                        ProductMediaURL = $"https://example.com/videos/{product.ProductId}/demo.mp4"
                    }
                });
            }

            await context.ProductMedias.AddRangeAsync(productMedia);
            await context.SaveChangesAsync();
        }

        private static async Task SeedRegions(AppDbContext context)
        {

            var regions = new List<Region>
            {
                new Region { RegionId = Guid.NewGuid(), RegionName = "Egypt" ,PoeRegion =1.15m,ConstTax=1.12m},
                new Region { RegionId = Guid.NewGuid(), RegionName = "Morocco" ,PoeRegion =1.5m,ConstTax=1.3m},
                new Region { RegionId = Guid.NewGuid(), RegionName = "England" ,PoeRegion =1.25m,ConstTax=1.5m},
                new Region { RegionId = Guid.NewGuid(), RegionName = "Saudi Arabia" ,PoeRegion =1.4m,ConstTax=1.3m},

            };

            await context.Regions.AddRangeAsync(regions);
            await context.SaveChangesAsync();
        }

        private static async Task SeedProductCustomAttributes(AppDbContext context)
        {
            var products = await context.Products.ToListAsync();
            var attributes = new List<ProductCustomAttribute>();

            foreach (var product in products)
            {
                if (product.ProductName.Contains("iPhone"))
                {
                    attributes.AddRange(new[]
                    {
                        new ProductCustomAttribute
                        {
                            ProductCustomAttributeId = Guid.NewGuid(),
                            Type = "Specification",
                            ProductId = product.ProductId,
                            Attribute = "Storage: 128GB"
                        },
                        new ProductCustomAttribute
                        {
                            ProductCustomAttributeId = Guid.NewGuid(),
                            Type = "Specification",
                            ProductId = product.ProductId,
                            Attribute = "Color: Titanium Black"
                        },
                        new ProductCustomAttribute
                        {
                            ProductCustomAttributeId = Guid.NewGuid(),
                            Type = "Feature",
                            ProductId = product.ProductId,
                            Attribute = "5G Compatible"
                        }
                    });
                }
                else if (product.ProductName.Contains("MacBook"))
                {
                    attributes.AddRange(new[]
                    {
                        new ProductCustomAttribute
                        {
                            ProductCustomAttributeId = Guid.NewGuid(),
                            Type = "Specification",
                            ProductId = product.ProductId,
                            Attribute = "Processor: M3 Pro"
                        },
                        new ProductCustomAttribute
                        {
                            ProductCustomAttributeId = Guid.NewGuid(),
                            Type = "Specification",
                            ProductId = product.ProductId,
                            Attribute = "RAM: 18GB"
                        },
                        new ProductCustomAttribute
                        {
                            ProductCustomAttributeId = Guid.NewGuid(),
                            Type = "Specification",
                            ProductId = product.ProductId,
                            Attribute = "Storage: 1TB SSD"
                        }
                    });
                }
                else if (product.ProductName.Contains("Nike") || product.ProductName.Contains("Adidas"))
                {
                    attributes.AddRange(new[]
                    {
                        new ProductCustomAttribute
                        {
                            ProductCustomAttributeId = Guid.NewGuid(),
                            Type = "Size",
                            ProductId = product.ProductId,
                            Attribute = "Available Sizes: 7-12"
                        },
                        new ProductCustomAttribute
                        {
                            ProductCustomAttributeId = Guid.NewGuid(),
                            Type = "Material",
                            ProductId = product.ProductId,
                            Attribute = "Upper: Knit Fabric"
                        },
                        new ProductCustomAttribute
                        {
                            ProductCustomAttributeId = Guid.NewGuid(),
                            Type = "Feature",
                            ProductId = product.ProductId,
                            Attribute = "Breathable"
                        }
                    });
                }
            }

            await context.ProductCustomAttributes.AddRangeAsync(attributes);
            await context.SaveChangesAsync();
        }

        private static async Task SeedReviews(AppDbContext context)
        {
            var products = await context.Products.ToListAsync();
            var reviews = new List<Review>();

            var random = new Random();
            foreach (var product in products)
            {
                for (int i = 0; i < 5; i++)
                {
                    reviews.Add(new Review
                    {
                        ReviewId = Guid.NewGuid(),
                        UserId = Guid.NewGuid(),
                        ProductId = product.ProductId,
                        FeedBackCreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 90)),
                        FeedBack = $"Great product! Exceeded my expectations. This is review #{i + 1} for {product.ProductName}",
                        Rate = random.Next(3, 6) // Ratings between 3-5
                    });
                }
            }

            await context.Reviews.AddRangeAsync(reviews);
            await context.SaveChangesAsync();
        }

        private static async Task SeedCartsAndWishlists(AppDbContext context)
        {
            var products = await context.Products.Take(3).ToListAsync();

            // Create carts
            var carts = new List<Cart>
            {
                new Cart { CartId = Guid.NewGuid(), UserId = Guid.NewGuid() },
                new Cart { CartId = Guid.NewGuid(), UserId = Guid.NewGuid() }
            };

            await context.Carts.AddRangeAsync(carts);
            await context.SaveChangesAsync();

            // Create cart products
            var cartProducts = new List<CartProduct>
            {
                new CartProduct { CartId = carts[0].CartId, ProductId = products[0].ProductId , Quantity = 2},
                new CartProduct { CartId = carts[0].CartId, ProductId = products[1].ProductId , Quantity = 1},
                new CartProduct { CartId = carts[1].CartId, ProductId = products[2].ProductId , Quantity = 3}
            };

            await context.CartProducts.AddRangeAsync(cartProducts);
            await context.SaveChangesAsync();

            // Create wishlists
            var wishlists = new List<WhishList>
            {
                new WhishList { WhishListId = Guid.NewGuid(), UserId = Guid.NewGuid() },
                new WhishList { WhishListId = Guid.NewGuid(), UserId = Guid.NewGuid() }
            };

            await context.WhishLists.AddRangeAsync(wishlists);
            await context.SaveChangesAsync();

            // Create wishlist products
            var wishlistProducts = new List<WhishListProduct>
            {
                new WhishListProduct { WhishListId = wishlists[0].WhishListId, ProductId = products[0].ProductId },
                new WhishListProduct { WhishListId = wishlists[0].WhishListId, ProductId = products[2].ProductId },
                new WhishListProduct { WhishListId = wishlists[1].WhishListId, ProductId = products[1].ProductId }
            };

            await context.WhishListProducts.AddRangeAsync(wishlistProducts);
            await context.SaveChangesAsync();
        }
    }
}
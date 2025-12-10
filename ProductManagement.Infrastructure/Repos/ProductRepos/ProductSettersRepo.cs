using Microsoft.EntityFrameworkCore;
using ProductManagement.Application.RepoContracts.IProductRepos;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure.DbContexts;

namespace ProductManagement.Infrastructure.Repos.ProductRepos
{
    public class ProductSettersRepo : IProductSettersRepo
    {
        private readonly AppDbContext _dbContext;

        public ProductSettersRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateNewProductAsync(Product productRequest)
        {
            if (productRequest.ProductId == Guid.Empty)
                productRequest.ProductId = Guid.NewGuid();
            await _dbContext.Products.AddAsync(productRequest);

            if (productRequest.ProductMedias != null && productRequest.ProductMedias.Any())
                await _dbContext.ProductMedias.AddRangeAsync(productRequest.ProductMedias!);

            if (productRequest.ProductCustomAttributes != null && productRequest.ProductCustomAttributes.Any())
                await _dbContext.ProductCustomAttributes.AddRangeAsync(productRequest.ProductCustomAttributes!);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Product product)
        {
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Guid productId, Product productRequest)
        {
            var existingProduct = await _dbContext.Products
                .Include(p => p.ProductMedias)
                .Include(p => p.ProductCustomAttributes)
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (existingProduct == null)
                return;

            // Update main product properties
            _dbContext.Entry(existingProduct).CurrentValues.SetValues(productRequest);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateProductMediaAsync(Guid mediaId, ProductMedia mediaRequest)
        {
            var existingMedia = await _dbContext.ProductMedias
                .FirstOrDefaultAsync(pm => pm.ProductMediaId == mediaId);

            if (existingMedia == null)
                return;

            existingMedia.Type = mediaRequest.Type;
            existingMedia.ProductMediaURL = mediaRequest.ProductMediaURL;

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateProductCustomAttributeAsync(Guid attributeId, ProductCustomAttribute attributeRequest)
        {
            var existingAttribute = await _dbContext.ProductCustomAttributes
                .FirstOrDefaultAsync(pca => pca.ProductCustomAttributeId == attributeId);

            if (existingAttribute == null)
                return;

            existingAttribute.Type = attributeRequest.Type;
            existingAttribute.Attribute = attributeRequest.Attribute;

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateProductReviewAsync(Guid reviewId, Review reviewRequest)
        {
            var existingReview = await _dbContext.Reviews
                .FirstOrDefaultAsync(r => r.ReviewId == reviewId);

            if (existingReview == null)
                return;

            existingReview.Rate = reviewRequest.Rate;
            existingReview.FeedBack = reviewRequest.FeedBack;
            existingReview.Likes = reviewRequest.Likes;
            existingReview.Dislikes = reviewRequest.Dislikes;

            await _dbContext.SaveChangesAsync();
        }

        public async Task AddVendor(Vendor vendor)
        {
            await _dbContext.Vendors.AddAsync(vendor);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsVendorAccessBrand(Guid vendorId, Guid brandId)
        {
            var brand = await _dbContext.Brands.Include(b=>b.Vendors).FirstOrDefaultAsync(b => b.BrandId == brandId);
            if (brand == null || brand.Vendors == null) return false;
            if (brand.Vendors.Any(v => v.VendorId == vendorId))
            {
                return true;
            }
            return false;
        }

        public async Task<bool> IsVendorExsist(Guid vendorId)
        {
            var vendor = await _dbContext.Vendors.FirstOrDefaultAsync(v => v.VendorId == vendorId);
            if (vendor == null) return false;
            return true;
        }
    }
}

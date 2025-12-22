using Microsoft.EntityFrameworkCore;
using ProductManagement.Application.RepoContracts.IProductRepos.Common;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure.DbContexts;

namespace ProductManagement.Infrastructure.Repos.ProductRepos.Common
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

        public async Task DeleteProductAsync(Guid productId)
        {
            var product = await _dbContext.Products
                .Include(p => p.ProductMedias)
                .Include(p => p.ProductCustomAttributes)
                .FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product != null)
            {
                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteProductMediaAsync(Guid mediaId)
        {
            var media = _dbContext.ProductMedias.FirstOrDefault(pm => pm.ProductMediaId == mediaId);
            if (media != null)
            {
                _dbContext.ProductMedias.Remove(media);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteProductCustomAttributeAsync(Guid attributeId)
        {
            var attribute = await _dbContext.ProductCustomAttributes
                .FirstOrDefaultAsync(pca => pca.ProductCustomAttributeId == attributeId);
            if (attribute != null)
            {
                _dbContext.ProductCustomAttributes.Remove(attribute);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteProductReviewAsync(Guid reviewId)
        {
            var review = await _dbContext.Reviews.FirstOrDefaultAsync(r => r.ReviewId == reviewId);
            if (review != null)
            {
                _dbContext.Reviews.Remove(review);
                await _dbContext.SaveChangesAsync();
            }
        }

    }
}

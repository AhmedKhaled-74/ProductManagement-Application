using Microsoft.EntityFrameworkCore;
using ProductManagement.Application.RepoContracts.IProductRepos.User;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure.DbContexts;

namespace ProductManagement.Infrastructure.Repos.ProductRepos.User
{
    public class ProductUserSettersRepo : IProductUserSetterRepo
    {
        private readonly AppDbContext _dbContext;
        public ProductUserSettersRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // ...implement methods from IProductUserSetterRepo...
        public async Task AddProductReviewAsync(Review reviewRequest)
        {
            await _dbContext.Reviews.AddAsync(reviewRequest);
        }

        public async Task UpdateProductReviewAsync(Guid reviewId, Review reviewRequest)
        {
            var existingReview = await _dbContext.Reviews
                .FirstOrDefaultAsync(r => r.ReviewId == reviewId);

            if (existingReview == null)
                return;

            existingReview.Rate = reviewRequest.Rate;
            existingReview.FeedBack = reviewRequest.FeedBack;

            await _dbContext.SaveChangesAsync();
        }

    }
}

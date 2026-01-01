using Microsoft.EntityFrameworkCore;
using ProductManagement.Application.RepoContracts.IProductRepos.User;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure.DbContexts;
using System.Linq.Expressions;

namespace ProductManagement.Infrastructure.Repos.ProductRepos.User
{
    public class ProductUserGettersRepo : IProductUserGetterRepo
    {
        private readonly AppReadDbContext _dbContext;
        public ProductUserGettersRepo(AppReadDbContext dbContext)
        {
            _dbContext = dbContext;
        }
       
        public async Task<List<Product>> GetFilteredProductsWithPaginationAsync(
                Expression<Func<Product, bool>>? filter,
                Expression<Func<Product, object>> orderBy,
                int pageNum,
                int pageSize)
        {
            var query = _dbContext.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.ProductSubCategory)
                .Include(p => p.ProductBrand)
                .Include(p => p.Vendor)
                .Include(p => p.ProductMedias)
                .AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query
                .OrderByDescending(orderBy)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<int> GetFilteredProductsCountAsync(Expression<Func<Product, bool>>? filter = null)
        {
            var query = _dbContext.Products
                .Include(p => p.ProductSubCategory)
                .Include(p => p.ProductCategory)
                .Include(p => p.Vendor)
                .Include(p => p.ProductBrand)
                .AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.CountAsync();
        }
        public async Task<Review?> GetProductReviewByIdsAsync(Guid productId , Guid userId)
        {
            return await _dbContext.Reviews
                .Include(r => r.Product)
                .FirstOrDefaultAsync(r => r.ProductId == productId && r.UserId == userId);
        }
        public async Task<Review?> GetProductReviewByIdAsync(Guid reviewId)
        {
            return await _dbContext.Reviews
                .Include(r => r.Product)
                .FirstOrDefaultAsync(r => r.ReviewId == reviewId);
        }
        public async Task<List<Product>> GetProductsSearchedWithPaginationAsync(Expression<Func<Product, bool>>? filter, int pageNum, int pageSize)
        {
            var query = _dbContext.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.ProductSubCategory)
                .Include(p => p.ProductBrand)
                .Include(p => p.Vendor)
                .Include(p => p.ProductMedias)
                .AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<List<Product>> GetOffersProductsAsync()
        {
            var query = _dbContext.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.ProductSubCategory)
                .Include(p => p.ProductBrand)
                .Include(p => p.Vendor)
                .Include(p => p.ProductMedias)
                .AsQueryable();
            query = query.Where(p => p.Discount > 0.15m);
            return await query.OrderByDescending(p => p.Discount)
                .Take(5)
                .ToListAsync();
        }
    }
}

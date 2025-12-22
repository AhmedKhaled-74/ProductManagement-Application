using Microsoft.EntityFrameworkCore;
using ProductManagement.Application.RepoContracts.IProductRepos.Admin;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure.DbContexts;

namespace ProductManagement.Infrastructure.Repos.ProductRepos.Admin
{
    public class ProductAdminGettersRepo : IProductAdminGetterRepo
    {
        private readonly AppReadDbContext _dbContext;
        public ProductAdminGettersRepo(AppReadDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // ...implement methods from IProductAdminGetterRepo...

        public async Task<List<Product>> GetAllProductsWithPaginationAsync(int pageNum, int pageSize)
        {
            return await _dbContext.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.ProductSubCategory)
                .Include(p => p.ProductBrand)
                .Include(p => p.Vendor)
                .Include(p => p.ProductMedias)
                .Include(p => p.ProductCustomAttributes)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<int?> GetAllProductsCountAsync()
        {
            return await _dbContext.Products.CountAsync();
        }
        public async Task<List<Product>> GetUnapprovedProductsWithPaginationAsync(int pageNum, int pageSize)
        {
            return await _dbContext.Products.IgnoreQueryFilters()
                .Include(p => p.ProductCategory)
                .Include(p => p.ProductSubCategory)
                .Include(p => p.ProductBrand)
                .Include(p => p.Vendor)
                .Include(p => p.ProductMedias)
                .Where(p => p.IsApproved == false)
                .OrderBy(p => p.CreatedAt)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<int?> GetUnapprovedProductsCountAsync()
        {
            return await _dbContext.Products.IgnoreQueryFilters()
                .Where(p => p.IsApproved == false)
                .CountAsync();
        }
    }
}

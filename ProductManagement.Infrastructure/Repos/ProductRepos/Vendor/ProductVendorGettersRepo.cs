using Microsoft.EntityFrameworkCore;
using ProductManagement.Application.RepoContracts.IProductRepos.Vendor;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure.DbContexts;

namespace ProductManagement.Infrastructure.Repos.ProductRepos.Vendor
{
    public class ProductVendorGettersRepo : IProductVendorGetterRepo
    {
        private readonly AppReadDbContext _dbContext;
        public ProductVendorGettersRepo(AppReadDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Methods for vendor product retrieval
        public async Task<List<Product>> GetProductsForVendorWithPaginationAsync(Guid vendorId, int pageNum, int pageSize)
        {
            return await _dbContext.Products.IgnoreQueryFilters()
                .Include(p => p.ProductCategory)
                .Include(p => p.ProductSubCategory)
                .Include(p => p.ProductBrand)
                .Include(p => p.Vendor)
                .Include(p => p.ProductMedias)
                .Where(p => p.VendorId == vendorId)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<int?> GetProductsForVendorCountAsync(Guid vendorId)
        {
            return await _dbContext.Products.IgnoreQueryFilters()
                .Where(p => p.VendorId == vendorId)
                .CountAsync();
        }
        public async Task<List<Product>> GetSearchedProductsForVendorWithPaginationAsync(Guid vendorId, string searchTerm, int pageNum, int pageSize)
        {
            return await _dbContext.Products.IgnoreQueryFilters()
                .Include(p => p.ProductCategory)
                .Include(p => p.ProductSubCategory)
                .Include(p => p.ProductBrand)
                .Include(p => p.Vendor)
                .Include(p => p.ProductMedias)
                .Where(p => p.VendorId == vendorId &&
                            (p.ProductName.Contains(searchTerm) ||
                             p.ProductDescription.Contains(searchTerm)))
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<int?> GetSearchedProductsForVendorCountAsync(Guid vendorId, string searchTerm)
        {
            return await _dbContext.Products.IgnoreQueryFilters()
                .Where(p => p.VendorId == vendorId &&
                            (p.ProductName.Contains(searchTerm) ||
                             p.ProductDescription.Contains(searchTerm)))
                .CountAsync();
        }
    }
}

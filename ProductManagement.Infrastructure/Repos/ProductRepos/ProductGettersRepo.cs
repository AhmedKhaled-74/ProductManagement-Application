using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ProductManagement.Application.RepoContracts.IProductRepos;
using ProductManagement.Domain.Entities;
using ProductManagement.Application.Helpers;
using ProductManagement.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Infrastructure.Repos.ProductRepos
{
    public class ProductGettersRepo : IProductGetterRepo
    {
        private readonly AppReadDbContext _dbContext;


        public ProductGettersRepo(AppReadDbContext dbContext)
        {
            _dbContext = dbContext;
        }

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
        public async Task<int> GetAllProductsCountAsync()
        {
            return await _dbContext.Products
            .CountAsync();
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
                .OrderByDescending(orderBy)   // now EF handles ordering
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();     // everything in SQL
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

        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            return await _dbContext.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.ProductSubCategory)
                .Include(p => p.ProductMedias)
                .Include(p => p.Vendor)
                .Include(p => p.Reviews)
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductCustomAttributes)
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<ProductMedia?> GetProductMediaByIdAsync(Guid id)
        {
            return await _dbContext.ProductMedias
                .Include(pm => pm.Product)
                .FirstOrDefaultAsync(pm => pm.ProductMediaId == id);
        }

        public async Task<ProductCustomAttribute?> GetProductCustomAttributeByIdAsync(Guid id)
        {
            return await _dbContext.ProductCustomAttributes
                .Include(pca => pca.Product)
                .FirstOrDefaultAsync(pca => pca.ProductCustomAttributeId == id);
        }

        public async Task<Review?> GetProductReviewByIdAsync(Guid id)
        {
            return await _dbContext.Reviews
                .Include(r => r.Product)
                .FirstOrDefaultAsync(r => r.ReviewId == id);
        }
    }
}

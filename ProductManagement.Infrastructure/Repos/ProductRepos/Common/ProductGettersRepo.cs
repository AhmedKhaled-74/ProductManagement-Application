using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ProductManagement.Domain.Entities;
using ProductManagement.Application.Helpers;
using ProductManagement.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ProductManagement.Application.RepoContracts.IProductRepos.Common;

namespace ProductManagement.Infrastructure.Repos.ProductRepos.Common
{
    public class ProductGettersRepo : IProductGetterRepo
    {
        private readonly AppReadDbContext _dbContext;


        public ProductGettersRepo(AppReadDbContext dbContext)
        {
            _dbContext = dbContext;
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
        public async Task<ProductCustomAttribute?> GetProductCustomAttributeByIdAsync(Guid id)
        {
            return await _dbContext.ProductCustomAttributes
                .Include(pca => pca.Product)
                .FirstOrDefaultAsync(pca => pca.ProductCustomAttributeId == id);
        }
        public async Task<ProductMedia?> GetProductMediaByIdAsync(Guid id)
        {
            return await _dbContext.ProductMedias
                .Include(pm => pm.Product)
                .FirstOrDefaultAsync(pm => pm.ProductMediaId == id);
        }


    }
}

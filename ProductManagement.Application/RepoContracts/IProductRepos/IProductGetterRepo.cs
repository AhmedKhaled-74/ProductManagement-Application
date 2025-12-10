using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.RepoContracts.IProductRepos
{
    public interface IProductGetterRepo
    {
        Task<List<Product>> GetAllProductsWithPaginationAsync(int pageNum, int pageSize);
        Task<int> GetAllProductsCountAsync();
        Task<int> GetFilteredProductsCountAsync(Expression<Func<Product, bool>>? filter = null);
        Task<List<Product>> GetFilteredProductsWithPaginationAsync(
                Expression<Func<Product, bool>>? filter,
                Expression<Func<Product, object>> orderBy,
                int pageNum,
                int pageSize);
        Task<Product?> GetProductByIdAsync(Guid id);
        Task<ProductMedia?> GetProductMediaByIdAsync(Guid id);
        Task<ProductCustomAttribute?> GetProductCustomAttributeByIdAsync(Guid id);
        Task<Review?> GetProductReviewByIdAsync(Guid id);

    }
}

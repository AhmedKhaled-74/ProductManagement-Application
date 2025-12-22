using ProductManagement.Domain.Entities;
using System.Linq.Expressions;
namespace ProductManagement.Application.RepoContracts.IProductRepos.User
{
    public interface IProductUserGetterRepo
    {
        Task<List<Product>> GetFilteredProductsWithPaginationAsync(Expression<Func<Product, bool>>? filter, Expression<Func<Product, object>> orderBy, int pageNum, int pageSize);
        Task<int> GetFilteredProductsCountAsync(Expression<Func<Product, bool>>? filter = null);
        Task<Review?> GetProductReviewByIdsAsync(Guid productId , Guid userId);
        Task<Review?> GetProductReviewByIdAsync(Guid reviewId);
        Task<List<Product>> GetProductsSearchedWithPaginationAsync(Expression<Func<Product, bool>>? filter, int pageNum, int pageSize);
        Task<List<Product>> GetOffersProductsAsync();
    }
}

using ProductManagement.Domain.Entities;
namespace ProductManagement.Application.RepoContracts.IProductRepos.Admin
{
    public interface IProductAdminGetterRepo
    {
        Task<List<Product>> GetAllProductsWithPaginationAsync(int pageNum, int pageSize);
        Task<int?> GetAllProductsCountAsync();
        Task<List<Product>> GetUnapprovedProductsWithPaginationAsync(int pageNum, int pageSize);
        Task<int?> GetUnapprovedProductsCountAsync();
    }
}

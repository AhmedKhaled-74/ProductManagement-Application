using ProductManagement.Domain.Entities;
namespace ProductManagement.Application.RepoContracts.IProductRepos.Vendor
{
    public interface IProductVendorGetterRepo
    {
        Task<List<Product>> GetProductsForVendorWithPaginationAsync(Guid vendorId, int pageNum, int pageSize);
        Task<int?> GetProductsForVendorCountAsync(Guid vendorId);
        Task<List<Product>> GetSearchedProductsForVendorWithPaginationAsync(Guid vendorId, string searchTerm, int pageNum, int pageSize);
        Task<int?> GetSearchedProductsForVendorCountAsync(Guid vendorId, string searchTerm);
    }
}

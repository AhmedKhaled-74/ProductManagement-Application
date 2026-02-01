using ProductManagement.Domain.Entities;
namespace ProductManagement.Application.RepoContracts.IProductRepos.Vendor
{
    public interface IProductVendorSetterRepo
    {
        Task CreateNewProductAsync(Product productRequest);
        Task UpdateProductAsync(Guid productId, Product productRequest);

        Task AddProductMediaAsync(ProductMedia mediaRequest);
        Task UpdateProductMediaAsync(Guid mediaId, ProductMedia mediaRequest);

        Task AddProductCustomAttributeAsync(ProductCustomAttribute attributeRequest);
        Task UpdateProductCustomAttributeAsync(Guid attributeId, ProductCustomAttribute attributeRequest);
        Task UpdateAllCustomAttributesByTypeAsync(Guid productId, string oldType, string newType);

        Task<bool> IsVendorAccessBrand(Guid vendorId, Guid brandId);
        Task<bool> IsVendorExsist(Guid vendorId);
        Task AddVendor(Domain.Entities.Vendor vendor);
    }
}

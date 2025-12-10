using ErrorOr;
using MediatR;
using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.RepoContracts.IProductRepos
{
    public interface IProductSettersRepo
    {
        Task CreateNewProductAsync(Product productRequest);
        Task DeleteProductAsync(Product productRequest);
        Task UpdateProductAsync(Guid productId, Product productRequest);
        Task UpdateProductMediaAsync(Guid mediaId, ProductMedia mediaRequest);
        Task UpdateProductCustomAttributeAsync(Guid attributeId, ProductCustomAttribute attributeRequest);
        Task UpdateProductReviewAsync(Guid reviewId, Review reviewRequest);
        Task<bool> IsVendorAccessBrand(Guid vendorId, Guid brandId);
        Task<bool> IsVendorExsist(Guid vendorId);
        Task AddVendor(Vendor vendor);
    }
}

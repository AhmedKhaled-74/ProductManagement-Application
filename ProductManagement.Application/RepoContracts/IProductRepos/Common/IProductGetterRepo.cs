using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.RepoContracts.IProductRepos.Common
{
    public interface IProductGetterRepo
    {
        Task<Product?> GetProductByIdAsync(Guid id);
        Task<ProductCustomAttribute?> GetProductCustomAttributeByIdAsync(Guid id);
        Task<ProductMedia?> GetProductMediaByIdAsync(Guid id);

    }
}

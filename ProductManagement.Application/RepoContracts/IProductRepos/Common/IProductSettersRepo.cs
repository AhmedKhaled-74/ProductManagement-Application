using ErrorOr;
using MediatR;
using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.RepoContracts.IProductRepos.Common
{
    public interface IProductSettersRepo
    {

        Task DeleteProductAsync(Guid productId);
        Task DeleteProductMediaAsync(Guid mediaId);
        Task DeleteProductCustomAttributeAsync(Guid attributeId);
        Task DeleteProductReviewAsync(Guid reviewId);

    }
}

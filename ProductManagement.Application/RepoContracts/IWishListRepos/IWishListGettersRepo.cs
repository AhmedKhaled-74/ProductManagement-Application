using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.RepoContracts.IWishListRepos
{
    public interface IWishListGettersRepo
    {
        Task<WhishList?> GetWishListByUserIdAsync(Guid userId);
        Task<bool> IsProductInWishListAsync(Guid userId, Guid productId);
    }
}

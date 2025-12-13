using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.RepoContracts.IWishListRepos
{
    public interface IWishListSettersRepo
    {
        Task AddProductToWishListAsync(Guid userId, Guid productId);
        Task RemoveProductFromWishListAsync(Guid userId, Guid productId);
        Task ClearWishList(Guid wishListId);
    }
}

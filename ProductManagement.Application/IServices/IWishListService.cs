using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.IServices
{
    public interface IWishListService
    {
        Task<ErrorOr<Success>> AddProductToWishListAsync(Guid? userId, Guid? productId);
        Task<ErrorOr<Success>> RemoveProductFromWishListAsync(Guid? userId, Guid? productId);
        Task<ErrorOr<bool>> IsProductInWishListAsync(Guid? userId, Guid? productId);
        Task<ErrorOr<Success>> ClearWishListAsync(Guid? wishListId);
    }
}

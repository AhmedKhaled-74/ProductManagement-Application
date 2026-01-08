using ErrorOr;
using ProductManagement.Application.DTOs;
using ProductManagement.Application.Helpers;

namespace ProductManagement.Application.IServices
{
    public interface IWishListService
    {
        Task<ErrorOr<WishListDTO>> GetWishListByUserIdAsync(Guid? userId, PriceConstsSetup? priceConstsSetup);
        Task<ErrorOr<Success>> AddProductToWishListAsync(Guid? userId, Guid? productId);
        Task<ErrorOr<Success>> RemoveProductFromWishListAsync(Guid? userId, Guid? productId);
        Task<ErrorOr<bool>> IsProductInWishListAsync(Guid? userId, Guid? productId);
        Task<ErrorOr<Success>> ClearWishListAsync(Guid? wishListId);
    }
}

using ErrorOr;
using ProductManagement.Application.DTOs;
using ProductManagement.Application.Errors;
using ProductManagement.Application.Helpers;
using ProductManagement.Application.IServices;
using ProductManagement.Application.Mappers;
using ProductManagement.Application.RepoContracts.IWishListRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Services
{
    public class WishListService(IWishListGettersRepo wishListGettersRepo, IWishListSettersRepo wishListSettersRepo) : IWishListService
    {
        private readonly IWishListGettersRepo _wishListGettersRepo = wishListGettersRepo;
        private readonly IWishListSettersRepo _wishListSettersRepo = wishListSettersRepo;

        public async Task<ErrorOr<Success>> AddProductToWishListAsync(Guid? userId, Guid? productId)
        {
            if (userId == null || productId == null)
            {
                
                return Errors.Errors.WishListErrors.WishListObjectRequired;
            }
            var isProductInWishList = await _wishListGettersRepo.IsProductInWishListAsync(userId.Value, productId.Value);
            if (isProductInWishList)
            {
                return Errors.Errors.WishListErrors.ProductAlreadyInWishList;
            }
            await _wishListSettersRepo.AddProductToWishListAsync(userId.Value, productId.Value);
            return Result.Success;
        }

        public async Task<ErrorOr<bool>> IsProductInWishListAsync(Guid? userId, Guid? productId)
        {
            if (userId == null || productId == null)
            {
                return Errors.Errors.WishListErrors.WishListObjectRequired;
            }
            return await _wishListGettersRepo.IsProductInWishListAsync(userId.Value, productId.Value);
        }

        public async Task<ErrorOr<Success>> RemoveProductFromWishListAsync(Guid? userId, Guid? productId)
        {
            if (userId == null || productId == null)
            {
                return Errors.Errors.WishListErrors.WishListObjectRequired;
            }
            var isProductInWishList = await _wishListGettersRepo.IsProductInWishListAsync(userId.Value, productId.Value);
            if (!isProductInWishList)
            {
                return Errors.Errors.WishListErrors.ProductNotInWishList;
            }
            await _wishListSettersRepo.RemoveProductFromWishListAsync(userId.Value, productId.Value);
            return Result.Success;
        }

        public async Task<ErrorOr<Success>> ClearWishListAsync(Guid? wishListId)
        {
            if (wishListId == null)
            {
                return Errors.Errors.WishListErrors.WishListObjectRequired;
            }
            await _wishListSettersRepo.ClearWishList(wishListId.Value);
            return Result.Success;
        }

        public async Task<ErrorOr<WishListDTO>> GetWishListByUserIdAsync(Guid? userId, PriceConstsSetup? priceConstsSetup)
        {
            if (userId == null || priceConstsSetup == null)
            {
                return Errors.Errors.WishListErrors.WishListObjectRequired;
            }
            var wishList =  await _wishListGettersRepo.GetWishListByUserIdAsync(userId.Value);
            if (wishList == null)
            {
                return Errors.Errors.WishListErrors.WishListNotFound;
            }
            return wishList.ToWishListDTO(priceConstsSetup);
        }
    }
}

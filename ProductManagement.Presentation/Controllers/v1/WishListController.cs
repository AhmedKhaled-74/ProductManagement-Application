using Microsoft.AspNetCore.Mvc;
using ProductManagement.Application.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Presentation.Controllers.v1
{
    public class WishListController(IWishListService wishListService) : CustomBaseController
    {
        private readonly IWishListService _wishListService = wishListService;

        /// <summary>
        /// method to add product to wish list
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpPost("wishlist/{userId}/add/{productId}")]
        public async Task<IActionResult> AddProductToWishList(Guid? userId, Guid? productId)
        {
            var result = await _wishListService.AddProductToWishListAsync(userId, productId);
            return result.Match(
                success => Ok(),
                errors => Problem(errors)
            );
        }

        /// <summary>
        /// method to remove product from wish list
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpDelete("wishlist/{userId}/remove/{productId}")]
        public async Task<IActionResult> RemoveProductFromWishList(Guid? userId, Guid? productId)
        {
            var result = await _wishListService.RemoveProductFromWishListAsync(userId, productId);
            return result.Match(
                success => Ok(),
                errors => Problem(errors)
            );
        }

        /// <summary>
        /// method to check if product is in wish list
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("wishlist/{userId}/contains/{productId}")]
        public async Task<IActionResult> IsProductInWishList(Guid? userId, Guid? productId)
        {
            var result = await _wishListService.IsProductInWishListAsync(userId, productId);
            return result.Match(
                isInWishList => Ok(isInWishList),
                errors => Problem(errors)
            );
        }

        /// <summary>
        /// method to clear wish list
        /// </summary>
        /// <param name="wishListId"></param>
        /// <returns></returns>
        [HttpDelete("wishlist/{wishListId}/clear")]
        public async Task<IActionResult> ClearWishList(Guid? wishListId)
        {
            var result = await _wishListService.ClearWishListAsync(wishListId);
            return result.Match(
                success => Ok(),
                errors => Problem(errors)
            );
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProductManagement.Application.Helpers;
using ProductManagement.Application.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Presentation.Controllers.v1
{
    public class WishListController: CustomBaseController
    {
        private readonly PriceConstsSetup _price;
        private static PriceConstsSetup? constsSetup;
        private readonly IWishListService _wishListService;
        public WishListController(IWishListService wishListService ,IOptions<PriceConstsSetup> priceConsts)
        {
            _wishListService = wishListService;
            _price = priceConsts.Value;
            constsSetup = new PriceConstsSetup
            {
                PoeRegion = 1.05m,
                CTax = 1.12m,
                CMass = _price.CMass,
                CVol = _price.CVol,
            };
        }

        /// <summary>
        /// method to get wish list by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetWishListByUserId(Guid? userId , PriceConstsSetup? priceConstsSetup)
        {
            priceConstsSetup = priceConstsSetup ?? WishListController.constsSetup;
            var result = await _wishListService.GetWishListByUserIdAsync(userId,priceConstsSetup);
            return result.Match(
                wishList => Ok(wishList),
                errors => Problem(errors)
            );
        }

        /// <summary>
        /// method to add product to wish list
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpPost("{userId}/add/{productId}")]
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
        [HttpDelete("{userId}/remove/{productId}")]
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
        [HttpGet("{userId}/contains/{productId}")]
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
        [HttpDelete("{wishListId}/clear")]
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

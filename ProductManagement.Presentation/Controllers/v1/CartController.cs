using MediatR;
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
    public class CartController: CustomBaseController
    {
        private readonly ICartService _cartService;
        private readonly PriceConstsSetup _price;
        private static PriceConstsSetup? constsSetup;
        public CartController(ICartService cartService, IOptions<PriceConstsSetup> priceConsts)
        {
            _cartService = cartService;
            _price = priceConsts.Value;
            constsSetup = new PriceConstsSetup
            {
                PoeRegion = 1.05m,
                CTax = 1.12m,
                CMass = _price.CMass,
                CVol = _price.CVol,
            };
            
        }
    // Controller actions and methods would go here


        /// <summary>
        ///  method to get cart by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="constsSetup"></param>
        /// <returns></returns>
        [HttpGet("cart/{userId}")]
        public async Task<IActionResult> GetCartByUserId(Guid userId , PriceConstsSetup? constsSetup)
        {
            var priceConstsSetup = constsSetup ?? CartController.constsSetup;

            var result = await _cartService.GetCartByUserId(userId , priceConstsSetup);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result);
        }

        /// <summary>
        ///  method to add product to cart
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("cart/{cartId}/add/{productId}")]
        public async Task<IActionResult> AddProductToCart(Guid productId, int quantity, Guid userId, PriceConstsSetup? constsSetup)
        {
            var priceConstsSetup = constsSetup ?? CartController.constsSetup;
            var result = await _cartService.AddProductToCart(productId, quantity, userId , priceConstsSetup);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// method to remove product from cart
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="userId"></param>
        /// <param name="constsSetup"></param>
        /// <returns></returns>
        [HttpDelete("cart/{cartId}/remove/{productId}")]
        public async Task<IActionResult> RemoveProductFromCart(Guid productId, Guid cartId)
        {    
            var result = await _cartService.RemoveProductFromCart(productId,cartId);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// method to clear cart
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("cart/{cartId}/clear")]
        public async Task<IActionResult> ClearCart(Guid cartId)
        {
            var result = await _cartService.ClearCart(cartId);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// method to update cart
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <param name="cartId"></param>
        /// <returns></returns>
        [HttpPut("cart/{cartId}/update/{productId}")]
        public async Task<IActionResult> UpdateCart(Guid productId, int quantity, Guid cartId)
        {
            var result = await _cartService.UpdateCart(productId, quantity, cartId);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// method to get cart by user id and cart id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cartId"></param>
        /// <param name="constsSetup"></param>
        /// <returns></returns>
        [HttpGet("cart/{userId}/{cartId}")]
        public async Task<IActionResult> GetCartByIds(Guid userId, Guid cartId, PriceConstsSetup? constsSetup)
        {
            var priceConstsSetup = constsSetup ?? CartController.constsSetup;
            var result = await _cartService.GetCartByIds(userId, cartId, priceConstsSetup);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result);
        }




    }
}

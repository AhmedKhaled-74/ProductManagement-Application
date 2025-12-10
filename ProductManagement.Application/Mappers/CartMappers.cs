using Mapster;
using ProductManagement.Application.DTOs.CartDTOs;
using ProductManagement.Application.Helpers;
using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Mappers
{
    public static class CartMappers
    {
        public static CartDTO ToCartDTO(this Cart cart, PriceConstsSetup priceConsts)
        {
            return new CartDTO
            {
                CartId = cart.CartId,
                UserId = cart.UserId,
                CartProducts = cart.CartProducts?.Select(cp => cp.ToCartProductDTO(priceConsts)).ToList()
            };
        }
        public static CartProductDTO ToCartProductDTO(this CartProduct cartProduct, PriceConstsSetup priceConsts)
        {
            var product = cartProduct.Product.ToProductResult(priceConsts);
            var cartProductDTO = product.Adapt<CartProductDTO>();
            cartProductDTO.CartId = cartProduct.CartId;
            cartProductDTO.ProductId = cartProduct.ProductId;
            cartProductDTO.Quantity = cartProduct.Quantity;
            return cartProductDTO;
        }
    }
}

using Mapster;
using ProductManagement.Application.DTOs.CartDTOs;
using ProductManagement.Application.DTOs.ProductDTOs.Result;
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
            if (cartProduct?.Product == null)
            {
                // Return default or throw appropriate exception based on your needs
                return new CartProductDTO();
            }

            var product = cartProduct.Product.ToProductResult(priceConsts);
            var cartProductDTO = product.Adapt<CartProductDTO>();
            cartProductDTO.CartProductId = cartProduct.CartProductId;
            cartProductDTO.CartId = cartProduct.CartId;
            cartProductDTO.ProductId = cartProduct.ProductId;
            cartProductDTO.Price = product.ActualProductPrice;
            cartProductDTO.Quantity = cartProduct.Quantity;

            // Handle null case for CartProductCustomAttributes
            if (cartProduct.CartProductCustomAttributes != null)
            {
                cartProductDTO.ProductCustomAttributes = cartProduct.CartProductCustomAttributes
                    .Where(cpa => cpa?.ProductCustomAttribute != null)
                    .Select(cpa => new ProductsCustomAttributeResult
                    {
                        ProductCustomAttributeId = cpa.ProductCustomAttribute.ProductCustomAttributeId,
                        Attribute = cpa.ProductCustomAttribute.Attribute,
                        Type = cpa.ProductCustomAttribute.Type,
                    }).ToList();
            }
            else
            {
                cartProductDTO.ProductCustomAttributes = new List<ProductsCustomAttributeResult>();
            }

            return cartProductDTO;
        }
    }
}

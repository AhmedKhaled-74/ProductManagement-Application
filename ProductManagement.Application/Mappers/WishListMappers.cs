using ProductManagement.Application.DTOs;
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
    public static class WishListMappers
    {
        public static WishListDTO ToWishListDTO(this WhishList wishList, PriceConstsSetup priceConsts)
        {
            return new WishListDTO
            {
                WhishListId = wishList.WhishListId,
                UserId = wishList.UserId,
                WishListProducts = wishList.WhishListProducts?.Select(wp => wp.ToWishListProductDTO(priceConsts)).ToList()
            };
        }
        public static WishListProductsDTO ToWishListProductDTO(this WhishListProduct wishListProduct, PriceConstsSetup priceConsts)
        {
            if (wishListProduct?.Product == null)
            {
                // Return default or throw appropriate exception based on your needs
                return new WishListProductsDTO();
            }
            var product = wishListProduct.Product;
            var wishListProductDTO = new WishListProductsDTO
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                ProductPrice = product.ToActualProductPrice(priceConsts),
                ProductImageUrl = product.ProductMedias?.FirstOrDefault()?.ProductMediaURL
            };
            return wishListProductDTO;
        }
    }
}

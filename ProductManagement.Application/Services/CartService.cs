using ErrorOr;
using Microsoft.Extensions.Options;
using ProductManagement.Application.DTOs.CartDTOs;
using ProductManagement.Application.Helpers;
using ProductManagement.Application.IServices;
using ProductManagement.Application.Mappers;
using ProductManagement.Application.RepoContracts.ICartRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Services
{
    public class CartService(ICartGettersRepo cartGettersRepo,ICartSettersRepo cartSettersRepo) : ICartService
    {
        private readonly ICartGettersRepo _cartGettersRepo = cartGettersRepo;
        private readonly ICartSettersRepo _cartSettersRepo = cartSettersRepo;

        public async Task<ErrorOr<Success>> AddProductToCart(Guid? ProductId, List<Guid>? customAttIds,int? Quantity, Guid? userId , PriceConstsSetup? priceConstsSetup)
        {
            if (ProductId is null || userId is null || priceConstsSetup is null || Quantity is null)
            {
                return Errors.Errors.CartErrors.CartObjectRequired;
            }
            var cart = await _cartGettersRepo.GetCartByUserIdAsync(userId.Value);
            if (cart is null)
            {
                 await _cartSettersRepo.CreateCart(userId.Value);
            }
            cart = await _cartGettersRepo.GetCartByUserIdAsync(userId.Value);
            if (cart is null)
            {
                return Errors.Errors.CartErrors.CartNotFound;
            }
            await _cartSettersRepo.AddProductToCart(ProductId.Value, customAttIds, Quantity.Value, cart.CartId);
            return Result.Success;
        }

        public async Task<ErrorOr<Success>> ClearCart(Guid? cartId)
        {
            if (cartId is null)
            {
                return Errors.Errors.CartErrors.CartObjectRequired;
            }
            await _cartSettersRepo.ClearCart(cartId.Value);
            return Result.Success;
        }

        public async Task<ErrorOr<List<CartProductDTO>>> SearchProductInCart(Guid? cartId, string? searchFor, PriceConstsSetup? priceConstsSetup)
        {
            if (cartId is null || string.IsNullOrEmpty(searchFor) || priceConstsSetup is null)
            {
                return Errors.Errors.CartErrors.CartObjectRequired;
            }
            var productsInCart = await _cartGettersRepo.GetSearchedProductsInCartAsync(cartId.Value, searchFor);

            var productsDTOs = productsInCart.Select(p => p.ToCartProductDTO(priceConstsSetup)).ToList();
            return productsDTOs;
        }

        public async Task<ErrorOr<CartDTO>> GetCartByIds(Guid? userId,Guid? cartId,PriceConstsSetup? priceConstsSetup)
        {
            if (userId is null || cartId is null || priceConstsSetup is null)
            {
                return Errors.Errors.CartErrors.CartObjectRequired;
            }
            var cart = await _cartGettersRepo.GetCartByIdsAsync(userId.Value, cartId.Value);
              if (cart is null)
              {
                return Errors.Errors.CartErrors.CartNotFound;
            }
            return cart.ToCartDTO(priceConstsSetup);
        }

        public async Task<ErrorOr<CartDTO>> GetCartByUserId(Guid? userId, PriceConstsSetup? priceConstsSetup)
        {
            if (userId is null ||  priceConstsSetup is null)
            {
                return Errors.Errors.CartErrors.CartObjectRequired;
            }
            var cart = await _cartGettersRepo.GetCartByUserIdAsync(userId.Value);
            if (cart is null)
            {
                return Errors.Errors.CartErrors.CartNotFound;
            }
            return cart.ToCartDTO(priceConstsSetup);
        }

        public async Task<ErrorOr<Success>> RemoveProductFromCart(Guid? ProductId, Guid? cartId)
        {
            if (ProductId is null || cartId is null)
            {
                return Errors.Errors.CartErrors.CartObjectRequired;
            }
            await _cartSettersRepo.RemoveProductFromCart(ProductId.Value, cartId.Value);
            return Result.Success;
        }

        public async  Task<ErrorOr<Success>> UpdateCart(Guid? CartProductId, List<Guid>? CustomAttributesIds , int? Quantity)
        {
            if (CartProductId is null || Quantity is null)
            {
                return Errors.Errors.CartErrors.CartObjectRequired;
            }
            var result = await _cartSettersRepo.UpdateCart(CartProductId.Value, CustomAttributesIds,Quantity.Value);
            if (result is null)
            {
                return Errors.Errors.CartErrors.FailedToUpdateCart;
            }
            return Result.Success;
        }
    }
}

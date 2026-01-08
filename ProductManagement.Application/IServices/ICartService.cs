using ErrorOr;
using ProductManagement.Application.DTOs.CartDTOs;
using ProductManagement.Application.Helpers;
using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.IServices
{
    public interface ICartService
    {
        Task<ErrorOr<Success>> AddProductToCart(Guid? ProductId, List<Guid>? customAttIds, int? Quantity,Guid? userId, PriceConstsSetup? priceConstsSetup);
        Task<ErrorOr<Success>> UpdateCart(Guid? CartProductId, List<Guid>? CustomAttributesIds, int? Quantity);
        Task<ErrorOr<List<CartProductDTO>>> SearchProductInCart(Guid? cartId, string? searchFor, PriceConstsSetup? priceConstsSetup);
        Task<ErrorOr<Success>> RemoveProductFromCart(Guid? ProductId, Guid? cartId);
        Task<ErrorOr<Success>> ClearCart(Guid? cartId);
        Task<ErrorOr<CartDTO>> GetCartByIds(Guid? userId, Guid? cartId, PriceConstsSetup? priceConstsSetup);
        Task<ErrorOr<CartDTO>> GetCartByUserId(Guid? userId, PriceConstsSetup? priceConstsSetup);

    }
}

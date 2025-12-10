using ErrorOr;
using ProductManagement.Application.DTOs.CartDTOs;
using ProductManagement.Application.Helpers;
using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.RepoContracts.ICartRepo
{
    public interface ICartSettersRepo
    {
        Task CreateCart(Guid userId);
        Task<CartProduct> AddProductToCart(Guid ProductId, int Quantity, Guid cartId);
        Task<CartProduct> UpdateCart(Guid ProductId, int Quantity, Guid cartId);
        Task RemoveProductFromCart(Guid ProductId, Guid cartId);
        Task ClearCart(Guid cartId);        
    }
}

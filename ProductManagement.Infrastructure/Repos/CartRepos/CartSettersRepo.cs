using Microsoft.EntityFrameworkCore;
using ProductManagement.Application.RepoContracts.ICartRepo;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Infrastructure.Repos.CartRepos
{
    public class CartSettersRepo(AppDbContext dbContext) : ICartSettersRepo
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<CartProduct> AddProductToCart(Guid ProductId, int Quantity, Guid cartId)
        {
            var cartProduct = new CartProduct
            {
                CartId = cartId,
                ProductId = ProductId,
                Quantity = Quantity
            };
            await _dbContext.CartProducts.AddAsync(cartProduct);
            await _dbContext.SaveChangesAsync();
            return cartProduct;
        }

        public async Task ClearCart(Guid cartId)
        {
            var cartProducts = _dbContext.CartProducts.Where(cp => cp.CartId == cartId);
            _dbContext.CartProducts.RemoveRange(cartProducts);
            await _dbContext.SaveChangesAsync();           
        }

        public async Task CreateCart(Guid userId)
        {
            await _dbContext.Carts.AddAsync(new Cart
            {
                CartId = Guid.NewGuid(),
                UserId = userId
            });
        }

        public async Task RemoveProductFromCart(Guid ProductId, Guid cartId)
        {
            var cartProduct = await _dbContext.CartProducts.FirstOrDefaultAsync(cp => cp.CartId == cartId && cp.ProductId == ProductId);
            if (cartProduct != null)
            {
                _dbContext.CartProducts.Remove(cartProduct);
                 await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<CartProduct> UpdateCart(Guid ProductId, int Quantity, Guid cartId)
        {
            var cartProduct = await  _dbContext.CartProducts.FirstOrDefaultAsync(cp => cp.CartId == cartId && cp.ProductId == ProductId);
            if (cartProduct != null)
            {
                cartProduct.Quantity = Quantity;
                _dbContext.CartProducts.Update(cartProduct);
                await _dbContext.SaveChangesAsync();
            }
            return cartProduct!;
        }
    }
}

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
    public class CartGettersRepo(AppDbContext dbContext) : ICartGettersRepo
    {
        private readonly AppDbContext _dbContext = dbContext;
        // Implement methods for getting cart data here

        public async Task<Cart?> GetCartByIdsAsync(Guid userId, Guid cartId)
        {
           return await _dbContext.Carts.FirstOrDefaultAsync(c=>c.CartId == cartId && c.UserId == userId);
        }
        public async Task<Cart?> GetCartByUserIdAsync(Guid userId)
        {
            return await _dbContext.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
        }
        public async Task<List<CartProduct>> GetSearchedProductsInCartAsync(Guid cartId, string searchFor)
        {
            return await _dbContext.CartProducts
                .Include(cp => cp.Product)
                .Where(cp => cp.CartId == cartId && (cp.Product.ProductName.Contains(searchFor) || cp.Product.ProductDescription.Contains(searchFor)))
                .ToListAsync();
        }
    }
}

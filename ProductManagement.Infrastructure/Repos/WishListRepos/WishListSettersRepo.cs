using Microsoft.EntityFrameworkCore;
using ProductManagement.Application.RepoContracts.IWishListRepos;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Infrastructure.Repos.WishListRepos
{
    public class WishListSettersRepo(AppDbContext dbContext) : IWishListSettersRepo
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task AddProductToWishListAsync(Guid userId, Guid productId)
        {
            var wishList = await _dbContext.WhishLists
                .FirstOrDefaultAsync(wl => wl.UserId == userId);
            if (wishList == null)
                {
                wishList = new WhishList
                {
                    WhishListId = Guid.NewGuid(),
                    UserId = userId,
                    WhishListProducts = new List<WhishListProduct>()
                };
                await _dbContext.WhishLists.AddAsync(wishList);
            }
            var wishListProduct = new WhishListProduct
            {
                WhishListId = wishList.WhishListId,
                ProductId = productId
            };
            await _dbContext.WhishListProducts.AddAsync(wishListProduct);
            await _dbContext.SaveChangesAsync();

        }

        public async Task RemoveProductFromWishListAsync(Guid userId, Guid productId)
        {
            var wishListProduct = _dbContext.WhishListProducts
                .Include(wlp => wlp.WhishList)
                .FirstOrDefault(wlp => wlp.WhishList.UserId == userId && wlp.ProductId == productId);
            if (wishListProduct != null)
            {
                _dbContext.WhishListProducts.Remove(wishListProduct);
                await _dbContext.SaveChangesAsync();
            }
            var wishList = await _dbContext.WhishLists
                .Include(wl => wl.WhishListProducts)
                .FirstOrDefaultAsync(wl => wl.UserId == userId);
            if (wishList != null && (wishList.WhishListProducts == null || !wishList.WhishListProducts.Any()))
            {
                _dbContext.WhishLists.Remove(wishList);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task ClearWishList(Guid wishListId)
        {
            var wishList = await _dbContext.WhishLists
                .Include(wl => wl.WhishListProducts)
                .FirstOrDefaultAsync(wl => wl.WhishListId == wishListId);
            if (wishList != null && wishList.WhishListProducts != null)
                {
                _dbContext.WhishListProducts.RemoveRange(wishList.WhishListProducts);
                await _dbContext.SaveChangesAsync();
            }
        }

    }
}

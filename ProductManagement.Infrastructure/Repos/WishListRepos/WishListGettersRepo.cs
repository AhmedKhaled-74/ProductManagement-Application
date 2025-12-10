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
    public class WishListGettersRepo(AppDbContext dbContext) : IWishListGettersRepo
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<WhishList?> GetWishListByUserIdAsync(Guid userId)
        {
            var wishList = await _dbContext.WhishLists
                .FirstOrDefaultAsync(wl => wl.UserId == userId);
            return wishList;
        }

        public Task<bool> IsProductInWishListAsync(Guid userId, Guid productId)
        {
           return _dbContext.WhishLists.Include(wl => wl.WhishListProducts)
                  .AnyAsync(wlp => wlp.UserId == userId &&
                  (wlp.WhishListProducts == null ? false : wlp.WhishListProducts.Any(p => p.ProductId == productId)));
        }
    }
}

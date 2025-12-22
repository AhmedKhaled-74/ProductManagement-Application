using Microsoft.EntityFrameworkCore;
using ProductManagement.Application.RepoContracts.IProductRepos.Admin;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure.DbContexts;

namespace ProductManagement.Infrastructure.Repos.ProductRepos.Admin
{
    public class ProductAdminSettersRepo : IProductAdminSetterRepo
    {
        private readonly AppDbContext _dbContext;
        public ProductAdminSettersRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ApproveProductAsync(Guid productId)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product != null)
            {
                product.IsApproved = true;
                await _dbContext.SaveChangesAsync();
            }
        }

    }
}

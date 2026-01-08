using Microsoft.EntityFrameworkCore;
using ProductManagement.Application.RepoContracts.ICartRepo;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure.DbContexts;

namespace ProductManagement.Infrastructure.Repos.CartRepos
{
    public class CartGettersRepo(AppDbContext dbContext) : ICartGettersRepo
    {
        private readonly AppDbContext _dbContext = dbContext;
        // Implement methods for getting cart data here

        public async Task<Cart?> GetCartByIdsAsync(Guid userId, Guid cartId)
        {
            return await _dbContext.Carts.FirstOrDefaultAsync(c => c.CartId == cartId && c.UserId == userId);
        }

        public async Task<Cart?> GetCartByUserIdAsync(Guid userId)
        {
            var query = _dbContext.Carts.AsQueryable();

            // Check if CartProducts property exists
            var cartProductsProperty = typeof(Cart).GetProperty("CartProducts");
            if (cartProductsProperty != null && cartProductsProperty.PropertyType.IsGenericType)
            {
                // Use a single Include chain to load all related data
                query = query.AsSplitQuery().Include(c => c.CartProducts!)
                    .ThenInclude(cp => cp.Product)
                        .ThenInclude(p => p.ProductCategory)
                    .Include(c => c.CartProducts!)
                    .ThenInclude(cp => cp.Product)
                        .ThenInclude(p => p.ProductSubCategory)
                    .Include(c => c.CartProducts!)
                    .ThenInclude(cp => cp.Product)
                        .ThenInclude(p => p.ProductBrand)
                    .Include(c => c.CartProducts!)
                    .ThenInclude(cp => cp.Product)
                        .ThenInclude(p => p.Vendor)
                    .Include(c => c.CartProducts!)
                    .ThenInclude(cp => cp.Product)
                        .ThenInclude(p => p.ProductMedias)
                    .Include(c => c.CartProducts!)
                    .ThenInclude(cp => cp.CartProductCustomAttributes)!
                        .ThenInclude(cpca => cpca.ProductCustomAttribute);
            }

            return await query
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<List<CartProduct>> GetSearchedProductsInCartAsync(Guid cartId, string searchFor)
        {
            if (string.IsNullOrWhiteSpace(searchFor))
            {
                return new List<CartProduct>();
            }

            var searchTerm = searchFor.Trim();

            return await _dbContext.CartProducts
                .AsSplitQuery()
                .Where(cp => cp.CartId == cartId)
                .Include(cp => cp.Product)
                    .ThenInclude(p => p.ProductCategory)
                .Include(cp => cp.Product)
                    .ThenInclude(p => p.ProductSubCategory)
                .Include(cp => cp.Product)
                    .ThenInclude(p => p.ProductBrand)
                .Include(cp => cp.Product)
                    .ThenInclude(p => p.Vendor)
                .Include(cp => cp.Product)
                    .ThenInclude(p => p.ProductMedias)
                .Include(cp => cp.CartProductCustomAttributes)!
                    .ThenInclude(cpca => cpca.ProductCustomAttribute)
                .Where(cp => cp.Product != null &&
                       EF.Functions.Like(cp.Product.ProductName, $"%{searchTerm}%"))
                .ToListAsync();
        }
    }
}

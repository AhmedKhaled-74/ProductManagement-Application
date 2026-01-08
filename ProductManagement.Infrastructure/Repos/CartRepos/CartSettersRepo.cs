using Microsoft.EntityFrameworkCore;
using ProductManagement.Application.RepoContracts.ICartRepo;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure.DbContexts;

namespace ProductManagement.Infrastructure.Repos.CartRepos
{
    public class CartSettersRepo(AppDbContext dbContext) : ICartSettersRepo
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task AddProductToCart(Guid ProductId, List<Guid>? CustomAttributesIds, int Quantity, Guid cartId)
        {
            var cartProductId = Guid.NewGuid();

            var product = await _dbContext.Products.Include(p => p.ProductCustomAttributes)
                .FirstOrDefaultAsync(p => p.ProductId == ProductId);

            if (product == null)
                throw new Exception($"Product with ID {ProductId} does not exist.");
            if (CustomAttributesIds != null)
            {
                if (product.ProductCustomAttributes == null)
                    throw new ArgumentException($"Product with ID {ProductId} has no custom attributes.");

                var typesOfAttributes = _dbContext.ProductCustomAttributes
                    .Where(ca => CustomAttributesIds.Contains(ca.ProductCustomAttributeId))
                    .Select(ca => ca.Type).ToList();

                if (typesOfAttributes.Count != typesOfAttributes.Distinct().Count())
                {
                    throw new ArgumentException("Duplicate custom attribute types are not allowed.");
                }

                foreach (var id in CustomAttributesIds)
                {
                    if (product.ProductCustomAttributes.Any(ca => ca.ProductId == ProductId))
                    {
                        var existsAtt = product.ProductCustomAttributes.FirstOrDefault(ca => ca.ProductCustomAttributeId == id);
                        continue;
                    }
                    else
                    {
                        throw new ArgumentException($"Custom Attribute with ID {id} does not exist for Product with ID {ProductId}.");

                    }
                }
            }

            var cartProduct = new CartProduct
            {
                CartProductId = cartProductId,
                CartId = cartId,
                ProductId = ProductId,
                Quantity = Quantity,
                CartProductCustomAttributes = CustomAttributesIds?.Select(caId => new CartProductCustomAttribute
                {
                    CartProductId = cartProductId,
                    ProductCustomAttributeId = caId
                }).ToList()
            };
            await _dbContext.CartProducts.AddAsync(cartProduct);
            await _dbContext.SaveChangesAsync();       
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
            await _dbContext.SaveChangesAsync();
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

        public async Task<CartProduct> UpdateCart(Guid cartProductId, List<Guid>? CustomAttributesIds, int Quantity)
        {
            var cartProduct = await _dbContext.CartProducts.Include(cp => cp.CartProductCustomAttributes)
                .FirstOrDefaultAsync(cp => cp.CartProductId == cartProductId);
            if (cartProduct != null)
            {
                if (CustomAttributesIds != null)
                {
                    var typesOfAttributes = _dbContext.ProductCustomAttributes
                            .Where(ca => CustomAttributesIds.Contains(ca.ProductCustomAttributeId))
                            .Select(ca => ca.Type).ToList();

                    if (typesOfAttributes.Count != typesOfAttributes.Distinct().Count())
                    {
                        throw new ArgumentException("Duplicate custom attribute types are not allowed.");
                    }
                    foreach (var item in CustomAttributesIds)
                    {
                        var existsAtt = await _dbContext.ProductCustomAttributes.FirstOrDefaultAsync(ca => ca.ProductCustomAttributeId == item);
                        if (existsAtt == null || existsAtt.ProductId != cartProduct.ProductId)
                        {
                            throw new Exception($"Custom Attribute with ID {item} does not exist.");

                        }
                    }
                }

                cartProduct.Quantity = Quantity;
                // Update custom attributes
                if (CustomAttributesIds != null)
                {
                    cartProduct.CartProductCustomAttributes?.Clear();
                    cartProduct.CartProductCustomAttributes = CustomAttributesIds.Select(caId => new CartProductCustomAttribute
                    {
                        CartProductId = cartProductId,
                        ProductCustomAttributeId = caId
                    }).ToList();
                }
                _dbContext.CartProducts.Update(cartProduct);
                await _dbContext.SaveChangesAsync();
            }
            return cartProduct!;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using ProductManagement.Application.RepoContracts.IProductRepos.Vendor;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure.DbContexts;

namespace ProductManagement.Infrastructure.Repos.ProductRepos.Vendor
{
    public class ProductVendorSettersRepo : IProductVendorSetterRepo
    {
        private readonly AppDbContext _dbContext;
        public ProductVendorSettersRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // ...existing methods...
        public async Task<bool> IsVendorAccessBrand(Guid vendorId, Guid brandId)
        {
            var brand = await _dbContext.Brands.Include(b => b.Vendors).FirstOrDefaultAsync(b => b.BrandId == brandId);
            if (brand == null || brand.Vendors == null) return false;
            return brand.Vendors.Any(v => v.VendorId == vendorId);
        }

        public async Task<bool> IsVendorExsist(Guid vendorId)
        {
            var vendor = await _dbContext.Vendors.FirstOrDefaultAsync(v => v.VendorId == vendorId);
            return vendor != null;
        }

        public async Task AddVendor(Domain.Entities.Vendor vendor)
        {
            await _dbContext.Vendors.AddAsync(vendor);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateNewProductAsync(Product productRequest)
        {
            if (productRequest.ProductId == Guid.Empty)
                productRequest.ProductId = Guid.NewGuid();
            await _dbContext.Products.AddAsync(productRequest);

            if (productRequest.ProductMedias != null && productRequest.ProductMedias.Any())
                await _dbContext.ProductMedias.AddRangeAsync(productRequest.ProductMedias!);

            if (productRequest.ProductCustomAttributes != null && productRequest.ProductCustomAttributes.Any())
                await _dbContext.ProductCustomAttributes.AddRangeAsync(productRequest.ProductCustomAttributes!);

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Guid productId, Product productRequest)
        {
            var existingProduct = await _dbContext.Products
                .Include(p => p.ProductMedias)
                .Include(p => p.ProductCustomAttributes)
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (existingProduct == null)
                return;

            existingProduct.IsApproved = false; // Set to false on update

            _dbContext.Entry(existingProduct).CurrentValues.SetValues(productRequest);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddProductMediaAsync(ProductMedia mediaRequest)
        {
            await _dbContext.ProductMedias.AddAsync(mediaRequest);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateProductMediaAsync(Guid mediaId, ProductMedia mediaRequest)
        {
            var existingMedia = await _dbContext.ProductMedias
                .FirstOrDefaultAsync(pm => pm.ProductMediaId == mediaId);

            if (existingMedia == null)
                return;

            existingMedia.Type = mediaRequest.Type;
            existingMedia.ProductMediaURL = mediaRequest.ProductMediaURL;

            var product = await _dbContext.Products
                .FirstOrDefaultAsync(p => p.ProductId == existingMedia.ProductId);
            if (product != null)
                product.IsApproved = false;

            await _dbContext.SaveChangesAsync();
        }

        public async Task AddProductCustomAttributeAsync(ProductCustomAttribute attributeRequest)
        {
            await _dbContext.ProductCustomAttributes.AddAsync(attributeRequest);
        }

        public async Task UpdateProductCustomAttributeAsync(Guid attributeId, ProductCustomAttribute attributeRequest)
        {
            var existingAttribute = await _dbContext.ProductCustomAttributes
                .FirstOrDefaultAsync(pca => pca.ProductCustomAttributeId == attributeId);

            if (existingAttribute == null)
                return;

            existingAttribute.Type = attributeRequest.Type;
            existingAttribute.Attribute = attributeRequest.Attribute;

            await _dbContext.SaveChangesAsync();
        }



    }
}

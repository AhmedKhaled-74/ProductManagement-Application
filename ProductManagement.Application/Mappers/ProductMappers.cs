using Mapster;
using ProductManagement.Application.DTOs.ProductDTOs;
using ProductManagement.Application.DTOs.ProductDTOs.AddRequest;
using ProductManagement.Application.DTOs.ProductDTOs.Result;
using ProductManagement.Application.DTOs.ProductDTOs.UpdateRequest;
using ProductManagement.Application.Helpers;
using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Mappers
{
    public static class ProductMappers
    {
        // 🔹 Add Request → Entity
        public static Product ToProductEntity(this ProductAddRequest productAdd)
        {
            var product = productAdd.Adapt<Product>();
            if (product.ProductId == Guid.Empty)
                product.ProductId = Guid.NewGuid();

            product.NumberOfRemainingProducts = productAdd.NumberOfAddingProducts;
            product.SoldTimes = 0;
            product.TotalRatedUsers = 0;
            product.TotalStars = 0;

            product.ProductMedias = productAdd.ProductMediaAddRequests?
                .Select(pm => pm.ToProductMediaEntity(product.ProductId)).ToList() ?? new List<ProductMedia>();

            product.ProductCustomAttributes = productAdd.ProductCustomAttributeAddRequests?
                .Select(pca => pca.ToProductCustomAttributeEntity(product.ProductId)).ToList() ?? new List<ProductCustomAttribute>();

            return product;
        }

        // 🔹 Generic internal method for Update Requests
        private static TEntity ToEntityInternal<TRequest, TEntity>(TRequest request, Guid productId)
            where TEntity : class, new()
        {
            var entity = request.Adapt<TEntity>();

            // Auto-generate primary key if empty
            var idProp = typeof(TEntity).GetProperties()
                .FirstOrDefault(p => p.Name.EndsWith("Id", StringComparison.OrdinalIgnoreCase) && p.PropertyType == typeof(Guid));

            if (idProp != null)
            {
                var currentValue = (Guid)idProp.GetValue(entity)!;
                if (currentValue == Guid.Empty)
                    idProp.SetValue(entity, Guid.NewGuid());
            }

            // Assign ProductId if it exists
            var productIdProp = typeof(TEntity).GetProperty("ProductId");
            if (productIdProp != null)
                productIdProp.SetValue(entity, productId);

            return entity;
        }

        // 🔹 Add + Update → Entity (Custom Attribute)
        public static ProductCustomAttribute ToProductCustomAttributeEntity(
            this ProductCustomAttributeAddRequest request, Guid productId)
            => ToEntityInternal<ProductCustomAttributeAddRequest, ProductCustomAttribute>(request, productId);

        public static ProductCustomAttribute ToProductCustomAttributeEntity(
            this ProductCustomAttributeUpdateRequest request, Guid productId)
            => ToEntityInternal<ProductCustomAttributeUpdateRequest, ProductCustomAttribute>(request, productId);

        // 🔹 Add + Update → Entity (Media)
        public static ProductMedia ToProductMediaEntity(
            this ProductMediaAddRequest request, Guid productId)
            => ToEntityInternal<ProductMediaAddRequest, ProductMedia>(request, productId);

        public static ProductMedia ToProductMediaEntity(
            this ProductMediaUpdateRequest request, Guid productId)
            => ToEntityInternal<ProductMediaUpdateRequest, ProductMedia>(request, productId);

        // 🔹 Add + Update → Entity (Review)
        public static Review ToReviewEntity(
            this ProductReviewAddRequest request, Guid productId)
            => ToEntityInternal<ProductReviewAddRequest, Review>(request, productId);

        public static Review ToReviewEntity(
            this ProductReviewUpdateRequest request, Guid productId)
            => ToEntityInternal<ProductReviewUpdateRequest, Review>(request, productId);

        // 🔹 Add + Update → Entity (Product)
        public static Product ToProductEntity(
            this ProductUpdateRequest request, Guid productId)
            => ToEntityInternal<ProductUpdateRequest, Product>(request, productId);

        public static Product ToProductEntity(
            this ProductAddRequest request, Guid productId)
            => ToEntityInternal<ProductAddRequest, Product>(request, productId);

        // 🔹 Product → Result
        public static ProductResult ToProductResult(this Product product, PriceConstsSetup priceConsts)
        {
            var productResult = product.Adapt<ProductResult>();

            productResult.BrandName = product.ProductBrand?.BrandName;
            productResult.VendorName = product.Vendor.VendorName;
            productResult.ProductCategoryName = product.ProductCategory.CategoryName;
            productResult.ProductSubCategoryName = product.ProductSubCategory?.SubCategoryName;
            productResult.IconUrl = product.ProductMedias.FirstOrDefault()!.ProductMediaURL;
            productResult.ActualProductPrice = product.ToActualProductPrice(priceConsts);
            productResult.Rate = product.TotalRatedUsers > 0
                ? Math.Round(((decimal)product.TotalStars / (product.TotalRatedUsers * 5)) * 5, 1)
                : 0;

            return productResult;
        }

        // 🔹 Product → Detailed Result
        public static ProductDetailedResult ToProductDetailedResult(this Product product, PriceConstsSetup priceConsts)
        {
            var productResult = product.Adapt<ProductDetailedResult>();

            productResult.BrandName = product.ProductBrand?.BrandName;
            productResult.VendorName = product.Vendor.VendorName;
            productResult.ProductCategoryName = product.ProductCategory.CategoryName;
            productResult.ProductSubCategoryName = product.ProductSubCategory?.SubCategoryName;

            productResult.ProductsCustomAttributesResults =
                product.ProductCustomAttributes?
                .Select(pca => pca.ToCustomProductAttributeResult()).ToList();

            productResult.ProductMediaResults =
                product.ProductMedias
                .Select(pm => pm.ToProductMediaResult()).ToList();

            productResult.IconUrl = product.ProductMedias.FirstOrDefault()!.ProductMediaURL;

            productResult.ProductsReviewsResults =
                product.Reviews?
                .Select(r => r.ToProductReviewResult()).ToList();

            productResult.ActualProductPrice = product.ToActualProductPrice(priceConsts);
            productResult.Rate = product.TotalRatedUsers > 0
                ? Math.Round(((decimal)product.TotalStars / (product.TotalRatedUsers * 5)) * 5, 1)
                : 0;

            return productResult;
        }

        // 🔹 Product → Actual Price
        public static decimal ToActualProductPrice(this Product product, PriceConstsSetup priceConsts)
        {
            var poe = product.ProductMass * priceConsts.CMass + product.ProductVolume * priceConsts.CVol;
            var priceWithoutTax = priceConsts.PoeRegion * (product.ProductPrice + poe) - (product.ProductPrice * product.Discount);
            return product.ProductPrice = Math.Round(priceWithoutTax * priceConsts.CTax, 3);
        }

        // 🔹 Entity → Result DTOs
        public static ProductsCustomAttributeResult ToCustomProductAttributeResult(this ProductCustomAttribute attribute)
            => attribute.Adapt<ProductsCustomAttributeResult>();

        public static ProductMediaResult ToProductMediaResult(this ProductMedia media)
            => media.Adapt<ProductMediaResult>();

        public static ProductsReviewResult ToProductReviewResult(this Review review)
            => review.Adapt<ProductsReviewResult>();
    }
}

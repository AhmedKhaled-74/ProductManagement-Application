using ErrorOr;
using MediatR;
using Microsoft.Extensions.Options;
using ProductManagement.Application.DTOs.ProductDTOs;
using ProductManagement.Application.Errors;
using ProductManagement.Application.Helpers;
using ProductManagement.Application.Mappers;
using ProductManagement.Application.Queries.ProductQueries;
using ProductManagement.Application.RepoContracts.IProductRepos;
using ProductManagement.Domain.Entities;
using System.Linq.Expressions;

namespace ProductManagement.Application.Handlers.QueriesHandlers.ProductQueriesHandlers
{
    public class GetFilteredProductsHandler : IRequestHandler<GetFilteredProductsQuery, ErrorOr<ProductsListResult>>
    {
        private readonly IProductGetterRepo _productGetterRepo;
        private readonly PaginationSetup _pagination;
        private readonly PriceConstsSetup _price;
        public GetFilteredProductsHandler(IProductGetterRepo productGetterRepo, IOptions<PaginationSetup> paginationOptions, IOptions<PriceConstsSetup> priceOptions)
        {
            _productGetterRepo = productGetterRepo;
            _pagination = paginationOptions.Value;
            _price = priceOptions.Value;
        }

        public async Task<ErrorOr<ProductsListResult>> Handle(GetFilteredProductsQuery request, CancellationToken cancellationToken)
        {
            if (request == null) return Errors.Errors.ProductErrors.ProductObjectRequired(nameof(request));
            if (request.orderBy == null) return Errors.Errors.ProductErrors.ProductObjectRequired(nameof(request.orderBy));

            int page = request.pageNum is null or <= 0 ? 1 : (int)request.pageNum;
            int size;
            if (request.role?.ToLower() == "admin")
            {
                size = _pagination.AdminPageSize ?? 100;
            }
            else
                size = _pagination.UserPageSize ?? 10;

            var priceConsts = new PriceConstsSetup
            {
                PoeRegion = request.poeRegion ?? 1.05m,
                CTax = request.cTax ?? 1.12m,
                CMass = _price.CMass,
                CVol = _price.CVol,
            };

            var priceExpr = ProductPriceExpressions.GetPriceExpression(priceConsts);

            // ORDER BY delegate
            Expression<Func<Product, object>> orderBy = request.orderBy switch
            {
                "Rate" => p => (p.TotalStars / (p.TotalRatedUsers * 5.0)) * 5.0,
                "Popular" => p => p.SoldTimes,
                "BestSelling" => p => p.SoldTimes,
                "Price" => Expression.Lambda<Func<Product, object>>(
                  Expression.Convert(priceExpr.Body, typeof(object)),
                  priceExpr.Parameters),
                "Category" => p => p.ProductCategoryId,
                "Offer" => p => p.Discount,
                _ => p => p.ProductId
            };

            string filterBy = request.filter?.ToLower() ?? string.Empty;

            // 🔹 Base filter for text
            Expression<Func<Product, bool>> textFilter = p =>
                 p.ProductName.ToLower().Contains(filterBy) ||
                 p.ProductCategory.CategoryName.ToLower().Contains(filterBy) ||
                 (
                 p.ProductSubCategory != null &&
                 p.ProductSubCategory.SubCategoryName.ToLower().Contains(filterBy)
                 );

            // 🔹 Start with text filter
            var filter = textFilter;

            // 🔹 Add price range filters dynamically

            if (request.minPrice.HasValue)
            {
                filter = filter.AndAlso(Expression.Lambda<Func<Product, bool>>(
                    Expression.GreaterThanOrEqual(priceExpr.Body, Expression.Constant(request.minPrice.Value)),
                    priceExpr.Parameters
                ));
            }

            if (request.maxPrice.HasValue)
            {
                filter = filter.AndAlso(Expression.Lambda<Func<Product, bool>>(
                    Expression.LessThanOrEqual(priceExpr.Body, Expression.Constant(request.maxPrice.Value)),
                    priceExpr.Parameters
                ));
            }

            var count =  _productGetterRepo.GetFilteredProductsCountAsync(filter);

            if ((page - 1) * size >= count.Result)
                return Errors.Errors.ProductErrors.FilteredProductsNotFound(page);

            var products = await _productGetterRepo.GetFilteredProductsWithPaginationAsync(
                filter,
                orderBy,
                page,
                size);

            return new ProductsListResult
            {
                Products = products.Select(p => p.ToProductResult(priceConsts)).ToList(),
                CurrentPage = page,
                PageSize = size,
                TotalCount = count.Result,
            };
        }
    }
}

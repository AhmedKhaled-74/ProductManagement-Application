using ErrorOr;
using MediatR;
using Microsoft.Extensions.Options;
using ProductManagement.Application.DTOs.ProductDTOs;
using ProductManagement.Application.DTOs.ProductDTOs.Result;
using ProductManagement.Application.Helpers;
using ProductManagement.Application.Mappers;
using ProductManagement.Application.Queries.ProductQueries;
using ProductManagement.Application.Queries.ProductQueries.User;
using ProductManagement.Application.RepoContracts.IProductRepos.User;
using ProductManagement.Domain.Entities;
using System.Linq.Expressions;

namespace ProductManagement.Application.Handlers.QueriesHandlers.ProductQueriesHandlers.User
{
    public class GetUserSearchProductsHandler : IRequestHandler<GetUserSearchProductsQuery, ErrorOr<ProductsListResult<ProductResult>>>
    {
        private readonly IProductUserGetterRepo _productGetterRepo;
        private readonly PaginationSetup _pagination;
        private readonly PriceConstsSetup _price;
        public GetUserSearchProductsHandler(IProductUserGetterRepo productGetterRepo, IOptions<PaginationSetup> paginationOptions, IOptions<PriceConstsSetup> priceOptions)
        {
            _productGetterRepo = productGetterRepo;
            _pagination = paginationOptions.Value;
            _price = priceOptions.Value;
        }

        public async Task<ErrorOr<ProductsListResult<ProductResult>>> Handle(GetUserSearchProductsQuery request, CancellationToken cancellationToken)
        {
            if (request == null) return Errors.Errors.ProductErrors.ProductObjectRequired(nameof(request));
            if (request.searchFor == null) return Errors.Errors.ProductErrors.ProductObjectRequired(nameof(request.searchFor));

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



            string filterBy = request.searchFor?.ToLower() ?? string.Empty;

            // ?? Base filter for text
            Expression<Func<Product, bool>> textFilter = p =>
                 p.ProductName.ToLower().Contains(filterBy) ||
                 p.ProductCategory.CategoryName.ToLower().Contains(filterBy) ||
                 (
                 p.ProductSubCategory != null &&
                 p.ProductSubCategory.SubCategoryName.ToLower().Contains(filterBy)
                 );

            // ?? Start with text filter
            var filter = textFilter;


            var count = _productGetterRepo.GetFilteredProductsCountAsync(filter);

            if ((page - 1) * size >= count.Result)
                return Errors.Errors.ProductErrors.FilteredProductsNotFound(page);

            var products = await _productGetterRepo.GetProductsSearchedWithPaginationAsync(
                filter,
                page,
                size);

            return new ProductsListResult<ProductResult>
            {
                Products = products.Select(p => p.ToProductResult(priceConsts)).ToList(),
                CurrentPage = page,
                PageSize = size,
                TotalCount = count.Result,
            };
        }
    }
}

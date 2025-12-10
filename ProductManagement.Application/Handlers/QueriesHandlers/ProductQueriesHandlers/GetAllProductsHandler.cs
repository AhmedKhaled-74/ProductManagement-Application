using ErrorOr;
using MediatR;
using Microsoft.Extensions.Options;
using ProductManagement.Application.DTOs.ProductDTOs;
using ProductManagement.Application.Errors;
using ProductManagement.Application.Helpers;
using ProductManagement.Application.Mappers;
using ProductManagement.Application.Queries.ProductQueries;
using ProductManagement.Application.RepoContracts.IProductRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Handlers.QueriesHandlers.ProductQueriesHandlers
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, ErrorOr<ProductsListResult>>
    {
        private readonly IProductGetterRepo _productGetterRepo;
        private readonly PaginationSetup _pagination;
        private readonly PriceConstsSetup _price;
        public GetAllProductsHandler(IProductGetterRepo productGetterRepo, IOptions<PaginationSetup> paginationOptions, IOptions<PriceConstsSetup> priceOptions)
        {
            _productGetterRepo = productGetterRepo;
            _pagination = paginationOptions.Value;
            _price = priceOptions.Value;
        }

        public async Task<ErrorOr<ProductsListResult>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Errors.Errors.ProductErrors.ProductObjectRequired(nameof(request));

            int page, size;

            page = request.pageNum <= 0 || request.pageNum == null ? 1 : (int)request.pageNum;
            if (request.role?.ToLower() == "admin")
            {
                size = _pagination.AdminPageSize??100;
            }else 
                size = _pagination.UserPageSize ?? 10;
            var count = _productGetterRepo.GetAllProductsCountAsync();

            if ((page - 1) * size >= count.Result)
                return Errors.Errors.ProductErrors.FilteredProductsNotFound(page);

            var products = await _productGetterRepo.GetAllProductsWithPaginationAsync(page, size);

            var priceConsts = new PriceConstsSetup()
            {
                PoeRegion = request.poeRegion ?? 1.05m,
                CTax = request.cTax ?? 1.12m,
                CMass = _price.CMass,
                CVol = _price.CVol,
            };

            return new ProductsListResult()
            {
                Products = products.Select(p => p.ToProductResult(priceConsts)).ToList(),
                CurrentPage = page,
                PageSize = size,
                TotalCount = count.Result,
            };
        }

   
    }
}

using ErrorOr;
using MediatR;
using Microsoft.Extensions.Options;
using ProductManagement.Application.DTOs.ProductDTOs;
using ProductManagement.Application.DTOs.ProductDTOs.Result;
using ProductManagement.Application.Helpers;
using ProductManagement.Application.Mappers;
using ProductManagement.Application.Queries.ProductQueries.Vendor;
using ProductManagement.Application.RepoContracts.IProductRepos.Common;
using ProductManagement.Application.RepoContracts.IProductRepos.User;
using ProductManagement.Application.RepoContracts.IProductRepos.Vendor;
using ProductManagement.Domain.Entities;
using System.Linq.Expressions;

namespace ProductManagement.Application.Handlers.QueriesHandlers.ProductQueriesHandlers.Vendor
{
    public class GetVendorProductsHandler : IRequestHandler<GetVendorProductsQuery, ErrorOr<ProductsListResult<ProductVendorResult>>>
    {
        private readonly IProductVendorGetterRepo _productVendorGetterRepo;
        private readonly PaginationSetup _pagination;
     
        public GetVendorProductsHandler(IProductVendorGetterRepo productGetterRepo, IOptions<PaginationSetup> paginationOptions)
        {
            _productVendorGetterRepo = productGetterRepo;
            _pagination = paginationOptions.Value;
        
        }
        public async Task<ErrorOr<ProductsListResult<ProductVendorResult>>> Handle(GetVendorProductsQuery request, CancellationToken cancellationToken)
        {
            int page = request.pageNum is null or <= 0 ? 1 : (int)request.pageNum;
            int size = _pagination.UserPageSize ?? 10;
            if (request == null) return Errors.Errors.ProductErrors.ProductObjectRequired(nameof(request));
            if (request?.vendorId == null) return Errors.Errors.ProductErrors.ProductObjectRequired(nameof(request.vendorId));
      
            var products = await _productVendorGetterRepo.GetProductsForVendorWithPaginationAsync(request.vendorId,page,size);
            var count = await _productVendorGetterRepo.GetProductsForVendorCountAsync(request.vendorId);
            if (products == null || count == 0 || count == null)
            {
                return new ProductsListResult<ProductVendorResult>
                {
                    Products = new List<ProductVendorResult>(),
                    CurrentPage = page,
                    PageSize = size,
                    TotalCount = 0
                };
            }
            return new  ProductsListResult<ProductVendorResult>
            {
                Products = products.Select(p => p.ToProductVendorResult()).ToList(),
                CurrentPage = request.pageNum ?? 1,
                PageSize = 10,
                TotalCount = count.Value
            };
        }
    }
}

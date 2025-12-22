using ErrorOr;
using MediatR;
using Microsoft.Extensions.Options;
using ProductManagement.Application.DTOs.ProductDTOs;
using ProductManagement.Application.DTOs.ProductDTOs.Result;
using ProductManagement.Application.Helpers;
using ProductManagement.Application.HttpClients;
using ProductManagement.Application.Mappers;
using ProductManagement.Application.Queries.ProductQueries.Admin;
using ProductManagement.Application.RepoContracts.IProductRepos.Admin;

namespace ProductManagement.Application.Handlers.QueriesHandlers.ProductQueriesHandlers.Admin
{
    public class GetUnapprovedProductsHandler : IRequestHandler<GetUnapprovedProductsQuery, ErrorOr<ProductsListResult<ProductVendorResult>>>
    {
        private readonly IProductAdminGetterRepo _productAdminGetterRepo;
        private readonly PaginationSetup _pagination;
        private readonly UserMicroClient _userClient;

        public GetUnapprovedProductsHandler(IProductAdminGetterRepo productAdminGetterRepo, IOptions<PaginationSetup> paginationOptions, UserMicroClient userClient)
        {
            _productAdminGetterRepo = productAdminGetterRepo;
            _pagination = paginationOptions.Value;
            _userClient = userClient;

        }
        public async Task<ErrorOr<ProductsListResult<ProductVendorResult>>> Handle(GetUnapprovedProductsQuery request, CancellationToken cancellationToken)
        {
            int page = request.pageNum is null or <= 0 ? 1 : (int)request.pageNum;
            int size = _pagination.AdminPageSize ?? 100;
            
            var adminUser = await _userClient.GetUserById(request.userId);
            if (adminUser == null || adminUser.RoleName != "Admin")
            {
                return Errors.Errors.AdminErrors.UserIsNotAdmin;
            }

            var products = await _productAdminGetterRepo.GetUnapprovedProductsWithPaginationAsync(page ,size);
            var count = await _productAdminGetterRepo.GetUnapprovedProductsCountAsync();    
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
            return new ProductsListResult<ProductVendorResult>
            {
                Products = products.Select(p => p.ToProductVendorResult()).ToList(),
                CurrentPage = page,
                PageSize = size,
                TotalCount = count.Value
            };
        }
    }
}

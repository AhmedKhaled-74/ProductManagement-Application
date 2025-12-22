using ErrorOr;
using MediatR;
using ProductManagement.Application.DTOs.ProductDTOs;
using ProductManagement.Application.DTOs.ProductDTOs.Result;

namespace ProductManagement.Application.Queries.ProductQueries.Admin
{
    public record GetUnapprovedProductsQuery(int? pageNum,Guid userId) : 
        IRequest<ErrorOr<ProductsListResult<ProductVendorResult>>>;
}

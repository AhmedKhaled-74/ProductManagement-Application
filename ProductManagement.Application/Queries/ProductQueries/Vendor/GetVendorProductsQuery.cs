using ErrorOr;
using MediatR;
using ProductManagement.Application.DTOs.ProductDTOs;
using ProductManagement.Application.DTOs.ProductDTOs.Result;

namespace ProductManagement.Application.Queries.ProductQueries.Vendor
{
    public record GetVendorProductsQuery(Guid vendorId,
        int? pageNum = 1) : IRequest<ErrorOr<ProductsListResult<ProductVendorResult>>>;
}

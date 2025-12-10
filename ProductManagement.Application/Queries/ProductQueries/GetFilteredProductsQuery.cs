using ErrorOr;
using MediatR;
using ProductManagement.Application.DTOs.ProductDTOs;

namespace ProductManagement.Application.Queries.ProductQueries
{
    public record GetFilteredProductsQuery(string? orderBy,
        string? filter,
        decimal? minPrice,
        decimal? maxPrice,
        string? role,
        int? pageNum = 1,
        decimal? poeRegion = 1.05m,
        decimal? cTax = 1.12m) : IRequest<ErrorOr<ProductsListResult>>;

}

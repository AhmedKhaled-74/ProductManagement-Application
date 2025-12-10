using ErrorOr;
using MediatR;
using ProductManagement.Application.DTOs.ProductDTOs.Result;

namespace ProductManagement.Application.Queries.ProductQueries
{
    public record GetProductByIdQuery(Guid? id ,
        decimal? poeRegion = 1.05m ,
        decimal? cTax = 1.12m)  : IRequest<ErrorOr<ProductDetailedResult>>;

}

using ErrorOr;
using MediatR;
using ProductManagement.Application.DTOs.ProductDTOs;
using ProductManagement.Application.DTOs.ProductDTOs.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Queries.ProductQueries.User
{
    public record GetOffersProductsQuery(
        decimal? poeRegion = 1.05m,
        decimal? cTax = 1.12m) : IRequest<ErrorOr<ProductsListResult<ProductResult>>>;   
    
}

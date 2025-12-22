using ErrorOr;
using MediatR;
using ProductManagement.Application.DTOs.ProductDTOs;
using ProductManagement.Application.DTOs.ReviewDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Queries.ProductQueries.User
{
    public record GetUserReviewForProductQuery(
    Guid? productId , Guid? userId) : IRequest<ErrorOr<ProductsReviewResult>>;

}

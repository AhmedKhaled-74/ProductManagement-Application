using ErrorOr;
using MediatR;
using ProductManagement.Application.DTOs.ProductDTOs.UpdateRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Commands.ProductCommands.User
{
    public record UpdateProductReviewCommand(ProductReviewUpdateRequest? updateRequest) : IRequest<ErrorOr<Success>>;

}

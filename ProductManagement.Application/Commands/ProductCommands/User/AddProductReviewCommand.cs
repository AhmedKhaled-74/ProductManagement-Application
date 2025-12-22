using ErrorOr;
using MediatR;
using ProductManagement.Application.DTOs.ProductDTOs.AddRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Commands.ProductCommands.User
{

    public record AddProductReviewCommand(ProductReviewAddRequest? AddRequest) : IRequest<ErrorOr<Success>>;
    
}

using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Commands.ProductCommands.User
{
    public record DeleteProductReviewCommand(Guid? reviewId , Guid? userId) : IRequest<ErrorOr<Success>>;
}

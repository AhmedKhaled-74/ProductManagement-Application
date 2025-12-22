using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Commands.ProductCommands.Vendor
{
    public record DeleteProductCommand(Guid productId , Guid userId) : IRequest<ErrorOr<Unit>>;

}

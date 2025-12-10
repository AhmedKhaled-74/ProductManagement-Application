using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Commands.ProductCommands
{
    public record DeleteProductCommand(Guid ProductId) : IRequest<ErrorOr<Unit>>;

}

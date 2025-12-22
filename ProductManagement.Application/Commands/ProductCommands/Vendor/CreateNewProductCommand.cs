using ErrorOr;
using MediatR;
using ProductManagement.Application.DTOs.ProductDTOs.AddRequest;
using ProductManagement.Application.DTOs.ProductDTOs.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Commands.ProductCommands.Vendor
{
    public record CreateNewProductCommand (ProductAddRequest productAddRequest) : IRequest<ErrorOr<Unit>>;
}

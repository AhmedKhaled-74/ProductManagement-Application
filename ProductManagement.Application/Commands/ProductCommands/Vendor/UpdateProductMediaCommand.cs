using ErrorOr;
using MediatR;
using ProductManagement.Application.DTOs.ProductDTOs.UpdateRequest;

namespace ProductManagement.Application.Commands.ProductCommands.Vendor
{
    public record UpdateProductMediaCommand (ProductMediaUpdateRequest productMediaUpdate,Guid userId) : IRequest<ErrorOr<Unit>>;
}

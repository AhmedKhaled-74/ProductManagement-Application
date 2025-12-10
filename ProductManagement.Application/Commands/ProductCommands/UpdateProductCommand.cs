using ErrorOr;
using MediatR;
using ProductManagement.Application.DTOs.ProductDTOs.UpdateRequest;

namespace ProductManagement.Application.Commands.ProductCommands
{
    public record UpdateProductCommand (ProductUpdateRequest ProductUpdateRequest) : IRequest<ErrorOr<Unit>>;
}

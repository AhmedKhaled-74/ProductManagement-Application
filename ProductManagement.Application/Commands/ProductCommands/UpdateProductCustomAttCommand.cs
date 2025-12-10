using ErrorOr;
using MediatR;
using ProductManagement.Application.DTOs.ProductDTOs.UpdateRequest;

namespace ProductManagement.Application.Commands.ProductCommands
{
    public record UpdateProductCustomAttCommand (ProductCustomAttributeUpdateRequest ProductCustomAttributeUpdateRequest) : IRequest<ErrorOr<Unit>>;
}

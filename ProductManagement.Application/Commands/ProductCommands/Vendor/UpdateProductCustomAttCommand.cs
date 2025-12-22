using ErrorOr;
using MediatR;
using ProductManagement.Application.DTOs.ProductDTOs.UpdateRequest;

namespace ProductManagement.Application.Commands.ProductCommands.Vendor
{
    public record UpdateProductCustomAttCommand (ProductCustomAttributeUpdateRequest productCustomAttributeUpdateRequest , Guid userId) : IRequest<ErrorOr<Unit>>;
}

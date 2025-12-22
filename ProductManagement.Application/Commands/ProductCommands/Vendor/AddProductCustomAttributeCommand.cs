using MediatR;
using ErrorOr;
using ProductManagement.Domain.Entities;
using ProductManagement.Application.DTOs.ProductDTOs.AddRequest;

namespace ProductManagement.Application.Commands.ProductCommands.Vendor
{
    public record AddProductCustomAttributeCommand(ProductCustomAttributeAddRequest productCustomAttribute , Guid userId) : IRequest<ErrorOr<Unit>>;
   
}

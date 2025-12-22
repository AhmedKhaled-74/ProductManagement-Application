using MediatR;
using ErrorOr;
using ProductManagement.Domain.Entities;
using ProductManagement.Application.DTOs.ProductDTOs.AddRequest;

namespace ProductManagement.Application.Commands.ProductCommands.Vendor
{
    public record AddProductMediaCommand(ProductMediaAddRequest productMedia, Guid userId) : IRequest<ErrorOr<Unit>>;

}

using ErrorOr;
using MediatR;
using ProductManagement.Application.DTOs.ProductDTOs.UpdateRequest;

namespace ProductManagement.Application.Commands.ProductCommands
{
    public record UpdateProductMediaCommand (ProductMediaUpdateRequest ProductMediaUpdate) : IRequest<ErrorOr<Unit>>;
}

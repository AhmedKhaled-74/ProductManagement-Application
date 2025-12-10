using ErrorOr;
using MediatR;
using ProductManagement.Application.DTOs.ProductDTOs.UpdateRequest;

namespace ProductManagement.Application.Commands.ProductCommands
{
    public record UpdateProductReviewCommand (ProductReviewUpdateRequest ProductReviewUpdateRequest) : IRequest<ErrorOr<Unit>>;
}

using MediatR;
using ErrorOr;

namespace ProductManagement.Application.Commands.ProductCommands.Admin
{
    public record DeleteProductReviewCommand(Guid reviewId,
    Guid userId) : IRequest<ErrorOr<Unit>>;
}

using MediatR;
using ErrorOr;

namespace ProductManagement.Application.Commands.ProductCommands.Admin
{
    public record ApproveProductCommand(Guid productId, Guid userId) : IRequest<ErrorOr<Unit>>;
}

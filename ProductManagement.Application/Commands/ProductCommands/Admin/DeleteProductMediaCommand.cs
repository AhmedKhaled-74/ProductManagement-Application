using MediatR;
using ErrorOr;

namespace ProductManagement.Application.Commands.ProductCommands.Admin
{
    public record DeleteProductMediaCommand(Guid productMediaId,
    Guid userId) : IRequest<ErrorOr<Unit>>;
}

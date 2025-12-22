using MediatR;
using ErrorOr;

namespace ProductManagement.Application.Commands.ProductCommands.Vendor
{
    public record DeleteProductMediaCommand(Guid productMediaId, Guid userId) : IRequest<ErrorOr<Unit>>;

}

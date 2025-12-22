using MediatR;
using ErrorOr;

namespace ProductManagement.Application.Commands.ProductCommands.Admin
{
    public record DeleteProductCustomAttributeCommand(Guid productCustomAttributeId,
    Guid userId ): IRequest<ErrorOr<Unit>>;
}

using ErrorOr;
using MediatR;
using ProductManagement.Application.Commands.ProductCommands.Admin;
using ProductManagement.Application.HttpClients;
using ProductManagement.Application.RepoContracts.IProductRepos.Common;

namespace ProductManagement.Application.Handlers.CommandsHandlers.ProductCommandsHandlers.Admin
{
    public class DeleteProductMediaHandler : IRequestHandler<DeleteProductMediaCommand, ErrorOr<Unit>>
    {
        private readonly IProductSettersRepo _settersRepo;
        private readonly UserMicroClient _userClient;

        public DeleteProductMediaHandler(IProductSettersRepo settersRepo, UserMicroClient userClient)
        {
            _settersRepo = settersRepo;
            _userClient = userClient;
        }
        public async Task<ErrorOr<Unit>> Handle(DeleteProductMediaCommand request, CancellationToken cancellationToken)
        {
            var adminUser = await _userClient.GetUserById(request.userId);
            if (adminUser == null || adminUser.RoleName != "Admin")
            {
                return Errors.Errors.AdminErrors.UserIsNotAdmin;
            }
            await _settersRepo.DeleteProductMediaAsync(request.productMediaId);
            return Unit.Value;
        }
    }
}

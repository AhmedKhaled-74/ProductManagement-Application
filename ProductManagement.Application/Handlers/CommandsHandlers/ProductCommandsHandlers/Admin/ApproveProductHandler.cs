using ErrorOr;
using MediatR;
using ProductManagement.Application.Commands.ProductCommands.Admin;
using ProductManagement.Application.HttpClients;
using ProductManagement.Application.RepoContracts.IProductRepos.Admin;

namespace ProductManagement.Application.Handlers.CommandsHandlers.ProductCommandsHandlers.Admin
{
    public class ApproveProductHandler : IRequestHandler<ApproveProductCommand, ErrorOr<Unit>>
    {
        private readonly IProductAdminSetterRepo _adminSetterRepo;
        private readonly UserMicroClient _userClient;
        public ApproveProductHandler(IProductAdminSetterRepo adminSetterRepo, UserMicroClient userClient)
        {
            _adminSetterRepo = adminSetterRepo;
            _userClient = userClient;
        }
        public async Task<ErrorOr<Unit>> Handle(ApproveProductCommand request, CancellationToken cancellationToken)
        {
            var adminUser = await _userClient.GetUserById(request.userId);
            if (adminUser == null || adminUser.RoleName != "Admin")
            {
                return Errors.Errors.AdminErrors.UserIsNotAdmin;
            }
            await _adminSetterRepo.ApproveProductAsync(request.productId);
            return Unit.Value;
        }
    }
}

using ErrorOr;
using MediatR;
using ProductManagement.Application.Commands.ProductCommands.Vendor;
using ProductManagement.Application.Helpers;
using ProductManagement.Application.HttpClients;
using ProductManagement.Application.RepoContracts.IProductRepos.Common;
using ProductManagement.Application.RepoContracts.IProductRepos.Vendor;

namespace ProductManagement.Application.Handlers.CommandsHandlers.ProductCommandsHandlers.Vendor
{
    public class DeleteProductMediaHandler : IRequestHandler<DeleteProductMediaCommand, ErrorOr<Unit>>
    {
        private readonly IProductSettersRepo _productSetterRepo;
        private readonly IProductGetterRepo _productGetterRepo;
        private readonly UserMicroClient _userClient;
        public DeleteProductMediaHandler(IProductSettersRepo productSetterRepo, IProductGetterRepo productGetterRepo, UserMicroClient userClient)
        {
            _productSetterRepo = productSetterRepo;
            _productGetterRepo = productGetterRepo;
            _userClient = userClient;
        }
        public async Task<ErrorOr<Unit>> Handle(DeleteProductMediaCommand request, CancellationToken cancellationToken)
        {
            var media = await _productGetterRepo.GetProductMediaByIdAsync(request.productMediaId);
            if (media == null)
                return Errors.Errors.ProductErrors.ProductMediaNotFound;
            var product = await _productGetterRepo.GetProductByIdAsync(media.ProductId);
            if (product == null)
                return Errors.Errors.ProductErrors.ProductNotFound;
            var vendorError = await VendorAuthorizationHelper.ValidateVendorAsync(_userClient, product.VendorId, request.userId);
            if (vendorError != null)
                return vendorError.Value;
            await _productSetterRepo.DeleteProductMediaAsync(request.productMediaId);
            return Unit.Value;
        }
    }
}

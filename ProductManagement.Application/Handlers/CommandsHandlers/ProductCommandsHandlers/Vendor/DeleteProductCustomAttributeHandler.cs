using ErrorOr;
using MediatR;
using ProductManagement.Application.Commands.ProductCommands.Vendor;
using ProductManagement.Application.Helpers;
using ProductManagement.Application.HttpClients;
using ProductManagement.Application.RepoContracts.IProductRepos.Common;
using ProductManagement.Application.RepoContracts.IProductRepos.Vendor;

namespace ProductManagement.Application.Handlers.CommandsHandlers.ProductCommandsHandlers.Vendor
{
    public class DeleteProductCustomAttributeHandler : IRequestHandler<DeleteProductCustomAttributeCommand, ErrorOr<Unit>>
    {
        private readonly IProductSettersRepo _productSetterRepo;
        private readonly IProductGetterRepo _productGetterRepo;
        private readonly UserMicroClient _userClient;
        public DeleteProductCustomAttributeHandler(IProductSettersRepo productSetterRepo, IProductGetterRepo productGetterRepo, UserMicroClient userClient)
        {
            _productSetterRepo = productSetterRepo;
            _productGetterRepo = productGetterRepo;
            _userClient = userClient;
        }
        public async Task<ErrorOr<Unit>> Handle(DeleteProductCustomAttributeCommand request, CancellationToken cancellationToken)
        {
            var attr = await _productGetterRepo.GetProductCustomAttributeByIdAsync(request.productCustomAttributeId);
            if (attr == null)
                return Errors.Errors.ProductErrors.ProductCustomAttributeNotFound;
            var product = await _productGetterRepo.GetProductByIdAsync(attr.ProductId);
            if (product == null)
                return Errors.Errors.ProductErrors.ProductNotFound;
            var vendorError = await VendorAuthorizationHelper.ValidateVendorAsync(_userClient, product.VendorId, request.userId);
            if (vendorError != null)
                return vendorError.Value;
            await _productSetterRepo.DeleteProductCustomAttributeAsync(request.productCustomAttributeId);
            return Unit.Value;
        }
    }
}

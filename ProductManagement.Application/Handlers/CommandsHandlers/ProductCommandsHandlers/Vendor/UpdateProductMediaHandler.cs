using ErrorOr;
using MediatR;
using ProductManagement.Application.Commands.ProductCommands.Vendor;
using ProductManagement.Application.HttpClients;
using ProductManagement.Application.Mappers;
using ProductManagement.Application.RepoContracts.IProductRepos.Common;
using ProductManagement.Application.RepoContracts.IProductRepos.Vendor;
using ProductManagement.Application.Helpers;

namespace ProductManagement.Application.Handlers.CommandsHandlers.ProductCommandsHandlers.Vendor
{
    public class UpdateProductMediaHandler : IRequestHandler<UpdateProductMediaCommand, ErrorOr<Unit>>
    {
        private readonly IProductVendorSetterRepo _productSetterRepo;
        private readonly IProductGetterRepo _productGetterRepo;
        private readonly UserMicroClient _userClient;

        public UpdateProductMediaHandler(IProductVendorSetterRepo productSettersRepo, IProductGetterRepo productGetterRepo,UserMicroClient userClient)
        {
            _productSetterRepo = productSettersRepo;
            _productGetterRepo = productGetterRepo;
            _userClient = userClient;
        }

        public async Task<ErrorOr<Unit>> Handle(UpdateProductMediaCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Errors.Errors.ProductErrors.ProductObjectRequired(nameof(request));

            var media = await _productGetterRepo.GetProductMediaByIdAsync(request.productMediaUpdate.ProductMediaId);
            if (media == null)
                return Errors.Errors.ProductErrors.ProductMediaNotFound;

            var product = await _productGetterRepo.GetProductByIdAsync(request.productMediaUpdate.ProductId);
            if (product == null)
                return Errors.Errors.ProductErrors.ProductNotFound;

            var vendorError = await VendorAuthorizationHelper.ValidateVendorAsync(_userClient, product.VendorId, request.userId);
            if (vendorError != null)
                return vendorError.Value;

            await _productSetterRepo.UpdateProductMediaAsync(request.productMediaUpdate.ProductMediaId, request.productMediaUpdate.ToProductMediaEntity(request.productMediaUpdate.ProductId));
            return Unit.Value;
        }
    }
}
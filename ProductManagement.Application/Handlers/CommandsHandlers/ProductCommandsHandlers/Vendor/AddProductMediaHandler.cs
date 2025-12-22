using ErrorOr;
using MediatR;
using ProductManagement.Application.Commands.ProductCommands.Vendor;
using ProductManagement.Application.Helpers;
using ProductManagement.Application.HttpClients;
using ProductManagement.Application.Mappers;
using ProductManagement.Application.RepoContracts.IProductRepos.Common;
using ProductManagement.Application.RepoContracts.IProductRepos.Vendor;

namespace ProductManagement.Application.Handlers.CommandsHandlers.ProductCommandsHandlers.Vendor
{
    public class AddProductMediaHandler : IRequestHandler<AddProductMediaCommand, ErrorOr<Unit>>
    {
        private readonly IProductVendorSetterRepo _productVendorSetterRepo;
        private readonly IProductGetterRepo _productGetterRepo;
        private readonly UserMicroClient _userClient;
        public AddProductMediaHandler(IProductVendorSetterRepo productVendorSetterRepo, IProductGetterRepo productGetterRepo, UserMicroClient userClient)
        {
            _productVendorSetterRepo = productVendorSetterRepo;
            _productGetterRepo = productGetterRepo;
            _userClient = userClient;
        }
        public async Task<ErrorOr<Unit>> Handle(AddProductMediaCommand request, CancellationToken cancellationToken)
        {
            var product = await _productGetterRepo.GetProductByIdAsync(request.productMedia.ProductId);
            if (product == null)
                return Errors.Errors.ProductErrors.ProductNotFound;
            var vendorError = await VendorAuthorizationHelper.ValidateVendorAsync(_userClient, product.VendorId, request.userId);
            if (vendorError != null)
                return vendorError.Value;
            await _productVendorSetterRepo.AddProductMediaAsync(request.productMedia.ToProductMediaEntity(product.ProductId));
            return Unit.Value;
        }
    }
}

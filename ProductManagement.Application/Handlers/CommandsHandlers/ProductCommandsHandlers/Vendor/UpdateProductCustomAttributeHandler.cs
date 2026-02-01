using ErrorOr;
using MediatR;
using ProductManagement.Application.Commands.ProductCommands.Vendor;
using ProductManagement.Application.Helpers;
using ProductManagement.Application.HttpClients;
using ProductManagement.Application.Mappers;
using ProductManagement.Application.RepoContracts.IProductRepos.Common;
using ProductManagement.Application.RepoContracts.IProductRepos.Vendor;
using System;

namespace ProductManagement.Application.Handlers.CommandsHandlers.ProductCommandsHandlers.Vendor
{
    public class UpdateProductCustomAttributeHandler(IProductVendorSetterRepo productSettersRepo, IProductGetterRepo productGetterRepo, UserMicroClient userClient) : IRequestHandler<UpdateProductCustomAttCommand, ErrorOr<Unit>>
    {
        private readonly IProductVendorSetterRepo _productSetterRepo = productSettersRepo;
        private readonly IProductGetterRepo _productGetterRepo = productGetterRepo;
        private readonly UserMicroClient _userClient = userClient;


        public async Task<ErrorOr<Unit>> Handle(UpdateProductCustomAttCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Errors.Errors.ProductErrors.ProductObjectRequired(nameof(request));

            var product = await _productGetterRepo.GetProductByIdAsync(request.productCustomAttributeUpdateRequest.ProductId);
            if (product == null)
                return Errors.Errors.ProductErrors.ProductNotFound;
            var attribute = await _productGetterRepo.GetProductCustomAttributeByIdAsync(request.productCustomAttributeUpdateRequest.ProductCustomAttributeId);
            if (attribute == null)
                return Errors.Errors.ProductErrors.ProductCustomAttributeNotFound;

            var vendorError = await VendorAuthorizationHelper.ValidateVendorAsync(_userClient, product.VendorId, request.userId);
            if (vendorError != null)
                return vendorError.Value;

            // Get the old type before updating
            var oldType = attribute.Type;
            var newType = request.productCustomAttributeUpdateRequest.Type;

            // Check if the type has changed
            if (!oldType.Equals(newType, StringComparison.OrdinalIgnoreCase))
            {
                // Update all custom attributes with the same old type to the new type
                await _productSetterRepo.UpdateAllCustomAttributesByTypeAsync(
                    product.ProductId, 
                    oldType, 
                    newType);
            }

            // Update the specific attribute (this will also update the Attribute value if it changed)
            await _productSetterRepo
                .UpdateProductCustomAttributeAsync(
                request.productCustomAttributeUpdateRequest.ProductCustomAttributeId,
                request.productCustomAttributeUpdateRequest
                .ToProductCustomAttributeEntity(request.productCustomAttributeUpdateRequest.ProductId));

            // TODO: Handle stock updates when type changes
            // If type changed, remove old stocks and create new stocks for all combinations
            // This will be implemented when Stock entity is ready

            return Unit.Value;
        }
    }
}
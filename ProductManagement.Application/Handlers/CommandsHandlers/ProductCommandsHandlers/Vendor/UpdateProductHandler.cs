using ErrorOr;
using MediatR;
using ProductManagement.Application.Commands.ProductCommands.Vendor;
using ProductManagement.Application.DTOs.ProductDTOs.UpdateRequest;
using ProductManagement.Application.Helpers;
using ProductManagement.Application.HttpClients;
using ProductManagement.Application.Mappers;
using ProductManagement.Application.RepoContracts;
using ProductManagement.Application.RepoContracts.IProductRepos.Common;
using ProductManagement.Application.RepoContracts.IProductRepos.Vendor;

namespace ProductManagement.Application.Handlers.CommandsHandlers.ProductCommandsHandlers.Vendor
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, ErrorOr<Unit>>
    {
        private readonly IProductVendorSetterRepo _productSetterRepo;
        private readonly ICategoryRepo _categoryRepo;
        private readonly IProductGetterRepo _productGetterRepo;
        private readonly UserMicroClient _userClient;

        public UpdateProductHandler(IProductVendorSetterRepo productSettersRepo, ICategoryRepo categoryRepo, IProductGetterRepo productGetterRepo ,UserMicroClient userClient)
        {
            _productSetterRepo = productSettersRepo;
            _categoryRepo = categoryRepo;
            _productGetterRepo = productGetterRepo;
            _userClient = userClient;
        }

        public async Task<ErrorOr<Unit>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Errors.Errors.ProductErrors.ProductObjectRequired(nameof(request));

            var product = await _productGetterRepo.GetProductByIdAsync(request.productUpdateRequest.ProductId);
            if (product == null)
                return Errors.Errors.ProductErrors.ProductNotFound;

            if (!await _categoryRepo.DoesCategoryExistAsync(request.productUpdateRequest.ProductCategoryId))
                return Errors.Errors.CategoryErrors.CategoryNotFound;

            if (request.productUpdateRequest.ProductSubCategoryId.HasValue && 
                !await _categoryRepo.DoesSubCategoryExistAsync(request.productUpdateRequest.ProductSubCategoryId.Value))
                return Errors.Errors.SubCategoryErrors.SubCategoryNotFound;

            var vendorError = await VendorAuthorizationHelper.ValidateVendorAsync(_userClient, product.VendorId, request.userId);
            if (vendorError != null)
                return vendorError.Value;

            await _productSetterRepo
                .UpdateProductAsync(request.productUpdateRequest.ProductId, 
                request.productUpdateRequest.ToProductEntity(request.productUpdateRequest.ProductId));

            // TODO: update stock for productname change
            return Unit.Value;
        }
    }
}
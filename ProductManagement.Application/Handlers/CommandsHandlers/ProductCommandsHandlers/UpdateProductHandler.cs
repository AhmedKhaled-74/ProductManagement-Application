using ErrorOr;
using MediatR;
using ProductManagement.Application.Commands.ProductCommands;
using ProductManagement.Application.DTOs.ProductDTOs.UpdateRequest;
using ProductManagement.Application.Mappers;
using ProductManagement.Application.RepoContracts;
using ProductManagement.Application.RepoContracts.IProductRepos;

namespace ProductManagement.Application.Handlers.CommandsHandlers.ProductCommandsHandlers
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, ErrorOr<Unit>>
    {
        private readonly IProductSettersRepo _productSetterRepo;
        private readonly ICategoryRepo _categoryRepo;
        private readonly IProductGetterRepo _productGetterRepo;

        public UpdateProductHandler(IProductSettersRepo productSettersRepo, ICategoryRepo categoryRepo, IProductGetterRepo productGetterRepo)
        {
            _productSetterRepo = productSettersRepo;
            _categoryRepo = categoryRepo;
            _productGetterRepo = productGetterRepo;
        }

        public async Task<ErrorOr<Unit>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Errors.Errors.ProductErrors.ProductObjectRequired(nameof(request));

            var product = await _productGetterRepo.GetProductByIdAsync(request.ProductUpdateRequest.ProductId);
            if (product == null)
                return Errors.Errors.ProductErrors.ProductNotFound;

            if (!await _categoryRepo.DoesCategoryExistAsync(request.ProductUpdateRequest.ProductCategoryId))
                return Errors.Errors.CategoryErrors.CategoryNotFound;

            if (request.ProductUpdateRequest.ProductSubCategoryId.HasValue && 
                !await _categoryRepo.DoesSubCategoryExistAsync(request.ProductUpdateRequest.ProductSubCategoryId.Value))
                return Errors.Errors.SubCategoryErrors.SubCategoryNotFound;

            await _productSetterRepo
                .UpdateProductAsync(request.ProductUpdateRequest.ProductId, 
                request.ProductUpdateRequest.ToProductEntity(request.ProductUpdateRequest.ProductId));
            return Unit.Value;
        }
    }
}
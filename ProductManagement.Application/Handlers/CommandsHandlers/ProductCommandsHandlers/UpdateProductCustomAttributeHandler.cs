using ErrorOr;
using MediatR;
using ProductManagement.Application.Commands.ProductCommands;
using ProductManagement.Application.Mappers;
using ProductManagement.Application.RepoContracts.IProductRepos;

namespace ProductManagement.Application.Handlers.CommandsHandlers.ProductCommandsHandlers
{
    public class UpdateProductCustomAttributeHandler(IProductSettersRepo productSettersRepo, IProductGetterRepo productGetterRepo) : IRequestHandler<UpdateProductCustomAttCommand, ErrorOr<Unit>>
    {
        private readonly IProductSettersRepo _productSetterRepo = productSettersRepo;
        private readonly IProductGetterRepo _productGetterRepo = productGetterRepo;

        public async Task<ErrorOr<Unit>> Handle(UpdateProductCustomAttCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Errors.Errors.ProductErrors.ProductObjectRequired(nameof(request));

            var product = await _productGetterRepo.GetProductByIdAsync(request.ProductCustomAttributeUpdateRequest.ProductId);
            if (product == null)
                return Errors.Errors.ProductErrors.ProductNotFound;
            var attribute = await _productGetterRepo.GetProductCustomAttributeByIdAsync(request.ProductCustomAttributeUpdateRequest.ProductCustomAttributeId);
            if (attribute == null)
                return Errors.Errors.ProductErrors.ProductCustomAttributeNotFound;


            await _productSetterRepo
                .UpdateProductCustomAttributeAsync(
                request.ProductCustomAttributeUpdateRequest.ProductCustomAttributeId , 
                request.ProductCustomAttributeUpdateRequest
                .ToProductCustomAttributeEntity(request.ProductCustomAttributeUpdateRequest.ProductId));
            return Unit.Value;
        }
    }
}
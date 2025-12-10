using ErrorOr;
using MediatR;
using ProductManagement.Application.Commands.ProductCommands;
using ProductManagement.Application.Mappers;
using ProductManagement.Application.RepoContracts.IProductRepos;

namespace ProductManagement.Application.Handlers.CommandsHandlers.ProductCommandsHandlers
{
    public class UpdateProductMediaHandler : IRequestHandler<UpdateProductMediaCommand, ErrorOr<Unit>>
    {
        private readonly IProductSettersRepo _productSetterRepo;
        private readonly IProductGetterRepo _productGetterRepo;

        public UpdateProductMediaHandler(IProductSettersRepo productSettersRepo, IProductGetterRepo productGetterRepo)
        {
            _productSetterRepo = productSettersRepo;
            _productGetterRepo = productGetterRepo;
        }

        public async Task<ErrorOr<Unit>> Handle(UpdateProductMediaCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Errors.Errors.ProductErrors.ProductObjectRequired(nameof(request));

            var media = await _productGetterRepo.GetProductMediaByIdAsync(request.ProductMediaUpdate.ProductMediaId);
            if (media == null)
                return Errors.Errors.ProductErrors.ProductMediaNotFound;

            var product = await _productGetterRepo.GetProductByIdAsync(request.ProductMediaUpdate.ProductId);
            if (product == null)
                return Errors.Errors.ProductErrors.ProductNotFound;

            await _productSetterRepo.UpdateProductMediaAsync(request.ProductMediaUpdate.ProductMediaId, request.ProductMediaUpdate.ToProductMediaEntity(request.ProductMediaUpdate.ProductId));
            return Unit.Value;
        }
    }
}
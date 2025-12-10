using ErrorOr;
using MediatR;
using ProductManagement.Application.Commands.ProductCommands;
using ProductManagement.Application.Mappers;
using ProductManagement.Application.RepoContracts.IProductRepos;

namespace ProductManagement.Application.Handlers.CommandsHandlers.ProductCommandsHandlers
{
    public class UpdateProductReviewHandler : IRequestHandler<UpdateProductReviewCommand, ErrorOr<Unit>>
    {
        private readonly IProductSettersRepo _productSetterRepo;
        private readonly IProductGetterRepo _productGetterRepo;

        public UpdateProductReviewHandler(IProductSettersRepo productSettersRepo, IProductGetterRepo productGetterRepo)
        {
            _productSetterRepo = productSettersRepo;
            _productGetterRepo = productGetterRepo;
        }

        public async Task<ErrorOr<Unit>> Handle(UpdateProductReviewCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Errors.Errors.ProductErrors.ProductObjectRequired(nameof(request));

            var review = await _productGetterRepo.GetProductReviewByIdAsync(request.ProductReviewUpdateRequest.ReviewId);
            if (review == null)
                return Errors.Errors.ProductErrors.ProductReviewNotFound;

            var product = await _productGetterRepo.GetProductByIdAsync(request.ProductReviewUpdateRequest.ProductId);
            if (product == null)
                return Errors.Errors.ProductErrors.ProductNotFound;

            await _productSetterRepo
                .UpdateProductReviewAsync(request.ProductReviewUpdateRequest.ReviewId, 
                request.ProductReviewUpdateRequest.ToReviewEntity(request.ProductReviewUpdateRequest.ProductId));
            return Unit.Value;
        }
    }
}
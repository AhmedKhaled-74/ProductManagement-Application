using ErrorOr;
using MediatR;
using Microsoft.Extensions.Options;
using ProductManagement.Application.DTOs.ProductDTOs.Result;
using ProductManagement.Application.Helpers;
using ProductManagement.Application.Mappers;
using ProductManagement.Application.Queries.ProductQueries.Common;
using ProductManagement.Application.RepoContracts.IProductRepos.Common;
using ProductManagement.Application.RepoContracts.IProductRepos.User;

namespace ProductManagement.Application.Handlers.QueriesHandlers.ProductQueriesHandlers.Common
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ErrorOr<ProductDetailedResult>>
    {
        private readonly IProductGetterRepo _productGetterRepo;
        private readonly PriceConstsSetup _price;

        public GetProductByIdHandler(IProductGetterRepo productGetterRepo, IOptions<PriceConstsSetup> priceOptions)
        {
            _productGetterRepo = productGetterRepo;
            _price = priceOptions.Value;
        }
        public async Task<ErrorOr<ProductDetailedResult>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            if (request == null) return Errors.Errors.ProductErrors.ProductObjectRequired(nameof(request));
            if (request.id == null) return Errors.Errors.ProductErrors.ProductIdRequired;

            var product =  await _productGetterRepo.GetProductByIdAsync((Guid)request.id);
            if (product == null)
                return Errors.Errors.ProductErrors.ProductNotFound;
            var priceConsts = new PriceConstsSetup()
            {
                PoeRegion = request.poeRegion ?? 1.05m,
                CTax = request.cTax ?? 1.12m,
                CMass = _price.CMass,
                CVol = _price.CVol,
            };
            var productRes = product.ToProductDetailedResult(priceConsts);
            return productRes;
        }
    }
}

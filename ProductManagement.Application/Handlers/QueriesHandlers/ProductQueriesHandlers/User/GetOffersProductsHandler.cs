using ErrorOr;
using MediatR;
using Microsoft.Extensions.Options;
using ProductManagement.Application.DTOs.ProductDTOs;
using ProductManagement.Application.DTOs.ProductDTOs.Result;
using ProductManagement.Application.Helpers;
using ProductManagement.Application.Mappers;
using ProductManagement.Application.Queries.ProductQueries.User;
using ProductManagement.Application.RepoContracts.IProductRepos.User;


namespace ProductManagement.Application.Handlers.QueriesHandlers.ProductQueriesHandlers.User
{
    public class GetOffersProductsHandler : IRequestHandler<GetOffersProductsQuery, ErrorOr<ProductsListResult<ProductResult>>>
    {
        private readonly IProductUserGetterRepo _productGetterRepo;
 
        private readonly PriceConstsSetup _price;
        public GetOffersProductsHandler(IProductUserGetterRepo productGetterRepo,IOptions<PriceConstsSetup> priceOptions)
        {
            _productGetterRepo = productGetterRepo;
            _price = priceOptions.Value;
        }
        public async Task<ErrorOr<ProductsListResult<ProductResult>>> Handle(GetOffersProductsQuery request, CancellationToken cancellationToken)
        {
            if (request == null) return Errors.Errors.ProductErrors.ProductObjectRequired(nameof(request));

            var priceConsts = new PriceConstsSetup
            {
                PoeRegion = request.poeRegion ?? 1.05m,
                CTax = request.cTax ?? 1.12m,
                CMass = _price.CMass,
                CVol = _price.CVol,
            };

            var priceExpr = ProductPriceExpressions.GetPriceExpression(priceConsts);

            var products = await _productGetterRepo.GetOffersProductsAsync();

            return new ProductsListResult<ProductResult>
            {
                Products = products.Select(p => p.ToProductResult(priceConsts)).ToList(),
                CurrentPage = 1,
                PageSize = 5,
                TotalCount = products.Count,
            };
        }
    }
}

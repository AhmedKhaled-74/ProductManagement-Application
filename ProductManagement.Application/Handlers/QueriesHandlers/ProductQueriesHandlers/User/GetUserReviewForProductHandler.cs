using ErrorOr;
using MediatR;
using Microsoft.Extensions.Options;
using ProductManagement.Application.DTOs.ProductDTOs;
using ProductManagement.Application.DTOs.ReviewDTOs;
using ProductManagement.Application.Helpers;
using ProductManagement.Application.Mappers;
using ProductManagement.Application.Queries.ProductQueries.User;
using ProductManagement.Application.RepoContracts.IProductRepos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Handlers.QueriesHandlers.ProductQueriesHandlers.User
{

    public class GetUserReviewForProductHandler : IRequestHandler<GetUserReviewForProductQuery, ErrorOr<ProductsReviewResult>>
    {
        private readonly IProductUserGetterRepo _productGetterRepo;

        public GetUserReviewForProductHandler(IProductUserGetterRepo productGetterRepo, IOptions<PaginationSetup> paginationOptions, IOptions<PriceConstsSetup> priceOptions)
        {
            _productGetterRepo = productGetterRepo;

        }

        public async Task<ErrorOr<ProductsReviewResult>> Handle(GetUserReviewForProductQuery request, CancellationToken cancellationToken)
        {
            if (request == null) return Errors.Errors.ProductErrors.ProductObjectRequired(nameof(request));
            if (request.productId == null) return Errors.Errors.ProductErrors.ProductIdRequired;
            if (request.userId == null) return Errors.Errors.ProductErrors.ProductObjectRequired(nameof(request.userId));
            var review = await _productGetterRepo.GetProductReviewByIdsAsync(request.productId.Value, request.userId.Value);
            if (review == null)
                return Errors.Errors.ReviewErrors.ReviewNotFound;
            var reviewDto = review.ToProductReviewResult();
            return reviewDto;
        }
    }
}

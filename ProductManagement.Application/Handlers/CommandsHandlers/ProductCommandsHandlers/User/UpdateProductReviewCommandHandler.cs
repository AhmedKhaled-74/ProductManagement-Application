using ErrorOr;
using MediatR;
using ProductManagement.Application.Commands.ProductCommands.User;
using ProductManagement.Application.HttpClients;
using ProductManagement.Application.Mappers;
using ProductManagement.Application.RepoContracts.IProductRepos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Handlers.CommandsHandlers.ProductCommandsHandlers.User
{
    public class UpdateProductReviewCommandHandler :IRequestHandler<UpdateProductReviewCommand, ErrorOr<Success>>
    {
        private readonly IProductUserSetterRepo _productSetterRepo;
        private readonly IProductUserGetterRepo _productGetterRepo;
        private readonly UserMicroClient _userClient;

        public UpdateProductReviewCommandHandler(IProductUserSetterRepo productSetterRepo, UserMicroClient userClient, IProductUserGetterRepo productGetterRepo)
        {
            _productSetterRepo = productSetterRepo;
            _userClient = userClient;
            _productGetterRepo = productGetterRepo;
        }

        public async Task<ErrorOr<Success>> Handle(UpdateProductReviewCommand request, CancellationToken cancellationToken)
        {
            if (request == null || request.updateRequest == null)
                return Errors.Errors.ReviewErrors.ReviewObjectRequired;
            var user = await _userClient.GetUserById(request.updateRequest.UserId);
            if (user == null)
                return Errors.Errors.ReviewErrors.UserNotFound;
            var review = await _productGetterRepo.GetProductReviewByIdsAsync(request.updateRequest.ProductId, request.updateRequest.UserId);
            if (review == null)
                return Errors.Errors.ReviewErrors.ReviewNotFound;
            if (review.ReviewId != request.updateRequest.ReviewId)
                return Errors.Errors.ReviewErrors.NotTheOwner;

            await _productSetterRepo.UpdateProductReviewAsync(review.ReviewId, request.updateRequest.ToReviewEntity(request.updateRequest.ProductId));
            return Result.Success;

        }
    }
}

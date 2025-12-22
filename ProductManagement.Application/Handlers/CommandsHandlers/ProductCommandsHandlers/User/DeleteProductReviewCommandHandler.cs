using ErrorOr;
using MediatR;
using ProductManagement.Application.Commands.ProductCommands.User;
using ProductManagement.Application.HttpClients;
using ProductManagement.Application.RepoContracts.IProductRepos.Common;
using ProductManagement.Application.RepoContracts.IProductRepos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Handlers.CommandsHandlers.ProductCommandsHandlers.User
{
  
    public class DeleteProductReviewCommandHandler : IRequestHandler<DeleteProductReviewCommand, ErrorOr<Success>>
    {
        private readonly IProductSettersRepo _productSetterRepo;
        private readonly IProductUserGetterRepo _productGetterRepo;
        private readonly UserMicroClient _userClient;

        public DeleteProductReviewCommandHandler(IProductSettersRepo productSetterRepo, UserMicroClient userClient, IProductUserGetterRepo productGetterRepo)
        {
            _productSetterRepo = productSetterRepo;
            _userClient = userClient;
            _productGetterRepo = productGetterRepo;
        }

        public async Task<ErrorOr<Success>> Handle(DeleteProductReviewCommand request, CancellationToken cancellationToken)
        {
            if (request == null || request.reviewId == null || request.userId == null)
                return Errors.Errors.ReviewErrors.ReviewObjectRequired;

            var hasReviewed = await _productGetterRepo.GetProductReviewByIdAsync(request.reviewId.Value);
            if (hasReviewed == null)
                return Errors.Errors.ReviewErrors.ReviewNotFound;

            var user = await _userClient.GetUserById(request.userId.Value);

            if (user == null)
                return Errors.Errors.ReviewErrors.UserNotFound;
            if (hasReviewed.UserId != request.userId.Value)
                return Errors.Errors.ReviewErrors.NotTheOwner;

            await _productSetterRepo.DeleteProductReviewAsync(request.reviewId.Value);
            return Result.Success;
        }
    }
}

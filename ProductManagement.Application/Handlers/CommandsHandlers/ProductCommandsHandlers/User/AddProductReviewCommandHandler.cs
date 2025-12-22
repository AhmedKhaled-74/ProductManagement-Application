using ErrorOr;
using MediatR;
using ProductManagement.Application.Commands.ProductCommands.User;
using ProductManagement.Application.HttpClients;
using ProductManagement.Application.Mappers;
using ProductManagement.Application.RepoContracts.IProductRepos.Common;
using ProductManagement.Application.RepoContracts.IProductRepos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Handlers.CommandsHandlers.ProductCommandsHandlers.User
{
    public class AddProductReviewCommandHandler : IRequestHandler<AddProductReviewCommand, ErrorOr<Success>>
    {
        private readonly IProductUserSetterRepo _productSetterRepo;
        private readonly IProductUserGetterRepo _productGetterRepo;
        private readonly UserMicroClient _userClient;

        public AddProductReviewCommandHandler(IProductUserSetterRepo productSetterRepo, UserMicroClient userClient, IProductUserGetterRepo productGetterRepo)
        {
            _productSetterRepo = productSetterRepo;
            _userClient = userClient;
            _productGetterRepo = productGetterRepo;
        }

        public async Task<ErrorOr<Success>> Handle(AddProductReviewCommand request, CancellationToken cancellationToken)
        {
            if (request == null || request.AddRequest == null)
                return Errors.Errors.ReviewErrors.ReviewObjectRequired;
            var user = await _userClient.GetUserById(request.AddRequest.UserId);
            if (user == null)
                return Errors.Errors.ReviewErrors.UserNotFound;
            var hasReviewed = await _productGetterRepo.GetProductReviewByIdsAsync(request.AddRequest.ProductId, request.AddRequest.UserId);
            if (hasReviewed != null)
                return Errors.Errors.ReviewErrors.UserAlreadyReviewed;

            await _productSetterRepo.AddProductReviewAsync(request.AddRequest.ToReviewEntity(request.AddRequest.ProductId));
            return Result.Success;

        }
    }
}

using ErrorOr;
using MediatR;
using ProductManagement.Application.Commands.ProductCommands.Vendor;
using ProductManagement.Application.Helpers;
using ProductManagement.Application.HttpClients;
using ProductManagement.Application.RepoContracts.IProductRepos.Common;
using ProductManagement.Application.RepoContracts.IProductRepos.Vendor;
using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Handlers.CommandsHandlers.ProductCommandsHandlers.Vendor
{
    public class DeleteProductHandler(IProductGetterRepo productGetterRepo, IProductSettersRepo productSetterRepo, UserMicroClient userClient) : IRequestHandler<DeleteProductCommand, ErrorOr<Unit>>
    {
       
        private readonly IProductSettersRepo _productSetterRepo = productSetterRepo;
        private readonly IProductGetterRepo _productGetterRepo = productGetterRepo;
        private readonly UserMicroClient _userClient = userClient;
        public async Task<ErrorOr<Unit>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Errors.Errors.ProductErrors.ProductObjectRequired(nameof(request));

            var productToDelete = await _productGetterRepo.GetProductByIdAsync(request.productId);
            if (productToDelete is null)
            {
                return Errors.Errors.ProductErrors.ProductNotFound;
            }

            var vendorError = await VendorAuthorizationHelper.ValidateVendorAsync(_userClient, productToDelete.VendorId, request.userId);
            if (vendorError != null)
                return vendorError.Value;

            await _productSetterRepo.DeleteProductAsync(productToDelete.ProductId);
            return Unit.Value;
        }
    }
}

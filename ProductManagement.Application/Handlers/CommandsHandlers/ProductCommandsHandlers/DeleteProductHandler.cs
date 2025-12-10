using ErrorOr;
using MediatR;
using ProductManagement.Application.Commands.ProductCommands;
using ProductManagement.Application.RepoContracts.IProductRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Handlers.CommandsHandlers.ProductCommandsHandlers
{
    public class DeleteProductHandler(IProductGetterRepo productGetterRepo , IProductSettersRepo productSettersRepo ) : IRequestHandler<DeleteProductCommand, ErrorOr<Unit>>
    {
        private readonly IProductSettersRepo _productSetterRepo = productSettersRepo;
        private readonly IProductGetterRepo _productGetterRepo = productGetterRepo;
        public async Task<ErrorOr<Unit>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Errors.Errors.ProductErrors.ProductObjectRequired(nameof(request));

            var productToDelete =  await _productGetterRepo.GetProductByIdAsync(request.ProductId);
            if (productToDelete is null)
            {
                return Errors.Errors.ProductErrors.ProductNotFound;
            }
            await _productSetterRepo.DeleteProductAsync(productToDelete);
            return Unit.Value;
        }
    }
}

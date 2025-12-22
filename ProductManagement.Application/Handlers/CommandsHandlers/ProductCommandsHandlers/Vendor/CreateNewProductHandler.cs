using ErrorOr;
using MediatR;
using ProductManagement.Application.Commands.ProductCommands.Vendor;
using ProductManagement.Application.HttpClients;
using ProductManagement.Application.Mappers;
using ProductManagement.Application.RepoContracts;
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
    public class CreateNewProductHandler : IRequestHandler<CreateNewProductCommand, ErrorOr<Unit>>
    {
        private readonly IProductVendorSetterRepo _productSetterRepo;
        private readonly ICategoryRepo _categoryRepo;
        private readonly UserMicroClient _userClient;

        public CreateNewProductHandler(IProductVendorSetterRepo productSettersRepo , ICategoryRepo categoryRepo , UserMicroClient userMicroClient)
        {
            _productSetterRepo = productSettersRepo;
            _categoryRepo = categoryRepo;
            _userClient = userMicroClient;

        }
        public async Task<ErrorOr<Unit>> Handle(CreateNewProductCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Errors.Errors.ProductErrors.ProductObjectRequired(nameof(request));
            if (!await _categoryRepo.DoesCategoryExistAsync(request.productAddRequest.ProductCategoryId))
                return Errors.Errors.CategoryErrors.CategoryNotFound;
            if (request.productAddRequest.ProductSubCategoryId.HasValue && !await _categoryRepo.DoesSubCategoryExistAsync(request.productAddRequest.ProductSubCategoryId.Value))
                return Errors.Errors.SubCategoryErrors.SubCategoryNotFound;

            var vendor = await _userClient.GetUserById(request.productAddRequest.VendorId);
            if (vendor == null)
                return Errors.Errors.VendorErrors.VendorNotFound;
            if (vendor.RoleName != "Vendor")
                return Errors.Errors.VendorErrors.UserIsNotVendor;

            if(request.productAddRequest.BrandId.HasValue)
            {
                var isVendorAccessBrand = await _productSetterRepo
                    .IsVendorAccessBrand(request.productAddRequest.VendorId, request.productAddRequest.BrandId.Value);
                if (!isVendorAccessBrand) return Errors.Errors.VendorErrors.VendorDoesntHaveBrandAuthorize;
            }
            if(!await _productSetterRepo.IsVendorExsist(vendor.UserId))
            {
                var addVendor = new Domain.Entities.Vendor { VendorId = vendor.UserId ,VendorName = vendor.FullName};
                await _productSetterRepo.AddVendor(addVendor);
            }
            await _productSetterRepo.CreateNewProductAsync(request.productAddRequest.ToProductEntity());
            return Unit.Value;
        }
    }
}

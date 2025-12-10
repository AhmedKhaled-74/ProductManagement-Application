using ErrorOr;
using MediatR;
using ProductManagement.Application.Commands.ProductCommands;
using ProductManagement.Application.HttpClients;
using ProductManagement.Application.Mappers;
using ProductManagement.Application.RepoContracts;
using ProductManagement.Application.RepoContracts.IProductRepos;
using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Handlers.CommandsHandlers.ProductCommandsHandlers
{
    public class CreateNewProductHandler : IRequestHandler<CreateNewProductCommand, ErrorOr<Unit>>
    {
        private readonly IProductSettersRepo _productSetterRepo;
        private readonly ICategoryRepo _categoryRepo;
        private readonly UserMicroClient _userClient;

        public CreateNewProductHandler(IProductSettersRepo productSettersRepo , ICategoryRepo categoryRepo , UserMicroClient userMicroClient)
        {
            _productSetterRepo = productSettersRepo;
            _categoryRepo = categoryRepo;
            _userClient = userMicroClient;

        }
        public async Task<ErrorOr<Unit>> Handle(CreateNewProductCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Errors.Errors.ProductErrors.ProductObjectRequired(nameof(request));
            if (!await _categoryRepo.DoesCategoryExistAsync(request.ProductAddRequest.ProductCategoryId))
                return Errors.Errors.CategoryErrors.CategoryNotFound;
            if (request.ProductAddRequest.ProductSubCategoryId.HasValue && !await _categoryRepo.DoesSubCategoryExistAsync(request.ProductAddRequest.ProductSubCategoryId.Value))
                return Errors.Errors.SubCategoryErrors.SubCategoryNotFound;

            var vendor = await _userClient.GetUserById(request.ProductAddRequest.VendorId);
            if (vendor == null)
                return Errors.Errors.VendorErrors.VendorNotFound;
            if (vendor.RoleName != "Vendor")
                return Errors.Errors.VendorErrors.UserIsNotVendor;

            if(request.ProductAddRequest.BrandId.HasValue)
            {
                var isVendorAccessBrand = await _productSetterRepo
                    .IsVendorAccessBrand(request.ProductAddRequest.VendorId, request.ProductAddRequest.BrandId.Value);
                if (!isVendorAccessBrand) return Errors.Errors.VendorErrors.VendorDoesntHaveBrandAuthorize;
            }
            if(!await _productSetterRepo.IsVendorExsist(vendor.UserId))
            {
                var addVendor = new Vendor { VendorId = vendor.UserId ,VendorName = vendor.FullName};
                await _productSetterRepo.AddVendor(addVendor);
            }
            await _productSetterRepo.CreateNewProductAsync(request.ProductAddRequest.ToProductEntity());
            return Unit.Value;
        }
    }
}

using ErrorOr;
using MediatR;
using ProductManagement.Application.Commands.ProductCommands.Vendor;
using ProductManagement.Application.DTOs.ProductDTOs.AddRequest;
using ProductManagement.Application.HttpClients;
using ProductManagement.Application.Mappers;
using ProductManagement.Application.RepoContracts;
using ProductManagement.Application.RepoContracts.IProductRepos.Common;
using ProductManagement.Application.RepoContracts.IProductRepos.Vendor;
using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
            var productEntity = request.productAddRequest.ToProductEntity();
            var productId = productEntity.ProductId;
            await _productSetterRepo.CreateNewProductAsync(productEntity);
            
            var productAddRequest = request.productAddRequest;
            if (productAddRequest.ProductCustomAttributeAddRequests != null && productAddRequest.ProductCustomAttributeAddRequests.Count > 0)
            {
                // Group attributes by Type
                var attributesByType = productAddRequest.ProductCustomAttributeAddRequests
                    .GroupBy(attr => attr.Type)
                    .ToDictionary(g => g.Key, g => g.Select(attr => attr.Attribute).Distinct().ToList());

                // Generate all combinations (Cartesian product)
                var combinations = GenerateAttributeCombinations(attributesByType);

                // Create stock for each combination
                // Example: For size [S,M,L] × color [R,B,G] × style [V,O]
                // This will create 18 stock instances:
                // - Stock 1: size=S, color=R, style=V
                // - Stock 2: size=S, color=R, style=O
                // - Stock 3: size=S, color=B, style=V
                // - Stock 4: size=S, color=B, style=O
                // - ... and so on for all combinations
                foreach (var combination in combinations)
                {
                    // TODO: Create stock instance for this combination
                    // Each combination contains a dictionary of Type -> Attribute value
                    // Example: { "size": "S", "color": "R", "style": "V" }
                    // Use productId and combination to create the stock entry
                    // Stock creation will be implemented later
                }
            }
            return Unit.Value;
        }

        /// <summary>
        /// Generates all combinations (Cartesian product) of custom attribute values.
        /// For example: size [S, M, L] × color [R, B, G] × style [V, O] 
        /// produces 3 × 3 × 2 = 18 combinations.
        /// </summary>
        /// <param name="attributesByType">Dictionary where key is attribute type (e.g., "size") and value is list of attribute values (e.g., ["S", "M", "L"])</param>
        /// <returns>List of dictionaries, each representing one combination of attributes</returns>
        private static List<Dictionary<string, string>> GenerateAttributeCombinations(
            Dictionary<string, List<string>> attributesByType)
        {
            if (attributesByType == null || attributesByType.Count == 0)
            return [];

            var combinations = new List<Dictionary<string, string>>();
            var types = attributesByType.Keys.ToList();
            var valueLists = types.Select(type => attributesByType[type]).ToList();

            // Generate Cartesian product using recursive approach
            GenerateCombinationsRecursive(types, valueLists, 0, [], combinations);

            return combinations;
        }

        private static void GenerateCombinationsRecursive(
            List<string> types,
            List<List<string>> valueLists,
            int currentIndex,
            Dictionary<string, string> currentCombination,
            List<Dictionary<string, string>> result)
        {
            if (currentIndex >= types.Count)
            {
                // Base case: we've processed all types, add this combination
                result.Add(new Dictionary<string, string>(currentCombination));
                return;
            }

            // For each value of the current type, recurse
            var currentType = types[currentIndex];
            var currentValues = valueLists[currentIndex];

            foreach (var value in currentValues)
            {
                currentCombination[currentType] = value;
                GenerateCombinationsRecursive(types, valueLists, currentIndex + 1, currentCombination, result);
            }
        }
    }
}

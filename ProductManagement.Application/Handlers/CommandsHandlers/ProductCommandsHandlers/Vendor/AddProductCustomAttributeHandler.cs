using ErrorOr;
using MediatR;
using ProductManagement.Application.Commands.ProductCommands.Vendor;
using ProductManagement.Application.Helpers;
using ProductManagement.Application.HttpClients;
using ProductManagement.Application.Mappers;
using ProductManagement.Application.RepoContracts.IProductRepos.Common;
using ProductManagement.Application.RepoContracts.IProductRepos.Vendor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductManagement.Application.Handlers.CommandsHandlers.ProductCommandsHandlers.Vendor
{
    public class AddProductCustomAttributeHandler : IRequestHandler<AddProductCustomAttributeCommand, ErrorOr<Unit>>
    {
        private readonly IProductVendorSetterRepo _productVendorSetterRepo;
        private readonly IProductGetterRepo _productGetterRepo;
        private readonly UserMicroClient _userClient;
        public AddProductCustomAttributeHandler(IProductVendorSetterRepo productVendorSetterRepo, IProductGetterRepo productGetterRepo, UserMicroClient userClient)
        {
            _productVendorSetterRepo = productVendorSetterRepo;
            _productGetterRepo = productGetterRepo;
            _userClient = userClient;
        }
        public async Task<ErrorOr<Unit>> Handle(AddProductCustomAttributeCommand request, CancellationToken cancellationToken)
        {
            var product = await _productGetterRepo.GetProductByIdAsync(request.productCustomAttribute.ProductId);
            if (product == null)
                return Errors.Errors.ProductErrors.ProductNotFound;
            var vendorError = await VendorAuthorizationHelper.ValidateVendorAsync(_userClient, product.VendorId, request.userId);
            if (vendorError != null)
                return vendorError.Value;

            // Check if this attribute type already exists
            var existingAttributes = product.ProductCustomAttributes?.ToList() ?? new List<Domain.Entities.ProductCustomAttribute>();
            var isNewType = !existingAttributes.Any(attr => attr.Type.Equals(request.productCustomAttribute.Type, StringComparison.OrdinalIgnoreCase));

            // Add the new custom attribute
            await _productVendorSetterRepo.AddProductCustomAttributeAsync(request.productCustomAttribute.ToProductCustomAttributeEntity(product.ProductId));

            // Handle stock creation based on whether type is new or existing
            if (isNewType)
            {
                // New type: Remove all old stocks and create new stocks for all combinations
                // Get all attributes including the newly added one
                var allAttributes = existingAttributes
                    .Concat(new[] { new Domain.Entities.ProductCustomAttribute 
                    { 
                        Type = request.productCustomAttribute.Type, 
                        Attribute = request.productCustomAttribute.Attribute 
                    }})
                    .ToList();

                // Group attributes by Type
                var attributesByType = allAttributes
                    .GroupBy(attr => attr.Type)
                    .ToDictionary(g => g.Key, g => g.Select(attr => attr.Attribute).Distinct().ToList());

                // Generate all combinations (Cartesian product)
                var combinations = GenerateAttributeCombinations(attributesByType);

                // TODO: Remove all existing stocks for this product
                // await _productVendorSetterRepo.DeleteAllStocksForProductAsync(product.ProductId);

                // Create stock for each combination
                foreach (var combination in combinations)
                {
                    // TODO: Create stock instance for this combination
                    // Each combination contains a dictionary of Type -> Attribute value
                    // Example: { "size": "S", "color": "R", "style": "V" }
                    // Stock creation will be implemented later
                }
            }
            else
            {
                // Existing type: Only create new stocks for the new attribute value combined with existing values of other types
                // Example: Adding "cyan" to color type when size [S,M,L] and style [V,O] exist
                // Creates: cyan+S+V, cyan+S+O, cyan+M+V, cyan+M+O, cyan+L+V, cyan+L+O

                // Get all existing attributes grouped by type
                var attributesByType = existingAttributes
                    .GroupBy(attr => attr.Type)
                    .ToDictionary(g => g.Key, g => g.Select(attr => attr.Attribute).Distinct().ToList());

                // Add the new attribute value to its type
                if (!attributesByType.ContainsKey(request.productCustomAttribute.Type))
                    attributesByType[request.productCustomAttribute.Type] = new List<string>();

                if (!attributesByType[request.productCustomAttribute.Type].Contains(request.productCustomAttribute.Attribute))
                    attributesByType[request.productCustomAttribute.Type].Add(request.productCustomAttribute.Attribute);

                // Generate combinations where the new attribute value is included
                // We only want combinations that include the new attribute value
                var newAttributeValue = request.productCustomAttribute.Attribute;
                var newAttributeType = request.productCustomAttribute.Type;

                // Create a modified dictionary where the new type only has the new value
                // This ensures we only generate combinations with the new value
                var combinationsForNewValue = GenerateCombinationsForNewAttributeValue(
                    attributesByType, 
                    newAttributeType, 
                    newAttributeValue);

                // Create stock for each combination
                foreach (var combination in combinationsForNewValue)
                {
                    // TODO: Create stock instance for this combination
                    // Each combination contains a dictionary of Type -> Attribute value
                    // Example: { "size": "S", "color": "cyan", "style": "V" }
                    // Stock creation will be implemented later
                }
            }

            return Unit.Value;
        }

        /// <summary>
        /// Generates combinations that include a specific new attribute value.
        /// For example, if adding "cyan" to color type, generates all combinations with cyan.
        /// </summary>
        private static List<Dictionary<string, string>> GenerateCombinationsForNewAttributeValue(
            Dictionary<string, List<string>> attributesByType,
            string newAttributeType,
            string newAttributeValue)
        {
            if (attributesByType == null || attributesByType.Count == 0)
                return [];

            var combinations = new List<Dictionary<string, string>>();
            var types = attributesByType.Keys.ToList();
            var valueLists = types.Select(type => 
            {
                // For the new attribute type, only include the new value
                if (type == newAttributeType)
                    return new List<string> { newAttributeValue };
                // For other types, include all their values
                return attributesByType[type];
            }).ToList();

            // Generate Cartesian product using recursive approach
            GenerateCombinationsRecursive(types, valueLists, 0, [], combinations);

            return combinations;
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

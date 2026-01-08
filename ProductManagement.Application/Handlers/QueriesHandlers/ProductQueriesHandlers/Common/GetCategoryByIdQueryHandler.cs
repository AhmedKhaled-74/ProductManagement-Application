using ErrorOr;
using MediatR;
using ProductManagement.Application.DTOs.CategoryDTOs;
using ProductManagement.Application.Queries.ProductQueries.Common;
using ProductManagement.Application.RepoContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Handlers.QueriesHandlers.ProductQueriesHandlers.Common
{
    public class GetCategoryByIdQueryHandler(ICategoryRepo categoryRepo) : IRequestHandler<GetCategoryByIdQuery, ErrorOr<CategoryDetailedResultDTO>>
    {
        public async Task<ErrorOr<CategoryDetailedResultDTO>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            if (request == null || !request.categoryId.HasValue) 
                return Errors.Errors.CategoryErrors.CategoryObjectRequired;

            var category = await categoryRepo.GetCategoryById(request.categoryId.Value);

            if (category == null)
            {
                return Errors.Errors.CategoryErrors.CategoryNotFound;
            }
            var categoryResult = new CategoryDetailedResultDTO
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                SubCategories = category.SubCategories?.Select(sc => new SubCategoryResultDTO
                {
                    SubCategoryId = sc.SubCategoryId,
                    SubCategoryName = sc.SubCategoryName,
                    CategoryId = sc.CategoryId
                }
                ).ToList(),
                CategoryDescription =  category.CategoryDescription,
                CategoryImageUrl = category.CategoryImageUrl
            };
            return categoryResult;

        }
    }
}

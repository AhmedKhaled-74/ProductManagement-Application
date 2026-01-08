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
    public class GetAllCategoriesQueryHandler(ICategoryRepo categoryRepo) : IRequestHandler<GetAllCategoriesQuery, ErrorOr<List<CategoryResultDTO>>>
    {

        public async Task<ErrorOr<List<CategoryResultDTO>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {        
            var categories = await categoryRepo.GetAllCategories();
            if (categories == null || !categories.Any())
            {
                return Errors.Errors.CategoryErrors.CategoryNotFound;
            }
            var categoryResults = categories.Select(c => new CategoryResultDTO
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                CategoryDescription = c.CategoryDescription,
                CategoryImageUrl = c.CategoryImageUrl
            }).ToList();
            return categoryResults;
        }
    }
}

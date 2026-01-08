using ErrorOr;
using MediatR;
using ProductManagement.Application.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Queries.ProductQueries.Common
{
    public record GetCategoryByIdQuery (Guid? categoryId)
        : IRequest<ErrorOr<CategoryDetailedResultDTO>>;

}

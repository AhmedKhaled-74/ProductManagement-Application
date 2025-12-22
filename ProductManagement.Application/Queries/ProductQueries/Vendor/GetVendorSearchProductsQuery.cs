using ErrorOr;
using MediatR;
using ProductManagement.Application.DTOs.ProductDTOs;
using ProductManagement.Application.DTOs.ProductDTOs.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Queries.ProductQueries.Vendor
{
    public record GetVendorSearchProductsQuery(Guid? vendorId,
    string? searchTerm,
    int? pageNum = 1) : IRequest<ErrorOr<ProductsListResult<ProductVendorResult>>>;

}

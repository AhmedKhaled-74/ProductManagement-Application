using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Application.DTOs;
using ProductManagement.Application.DTOs.ProductDTOs;
using ProductManagement.Application.IServices;
using ProductManagement.Application.Queries.ProductQueries;

namespace ProductManagement.Presentation.Controllers.v1
{
    [Route("api/v{version:int}/Products")]
    [ApiVersion("1.0")]

    public class ProductGetterController : CustomBaseController
    {
        private readonly ISender _mediator;
        private readonly IRegionService _regionService;

        public ProductGetterController(ISender mediator ,IRegionService regionService)
        {
            _mediator = mediator;
            _regionService = regionService;
        }

        /// <summary>
        /// Get all products with pagination
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllProducts(
            [FromQuery] int? pageNum ,
            [FromQuery] string? role ,
            [FromQuery] string? region)
        {
            var regionProps = await _regionService.GetRegionPropsByNameAsync(region);
            if (regionProps.IsError)
            {
                return Problem(regionProps.Errors);
            }
            var query = new GetAllProductsQuery(role,pageNum, regionProps.Value.PoeRegion, regionProps.Value.ConstTax);
            var result = await _mediator.Send(query);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Get filtered products with optional order, filter, and price range
        /// </summary>
        [HttpGet("filtered")]
        public async Task<IActionResult> GetFilteredProducts(
            [FromQuery] string? orderBy,
            [FromQuery] string? filter,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] int? pageNum,
            [FromQuery] string? role,
            [FromQuery] string? region)
        {
            var regionProps = await _regionService.GetRegionPropsByNameAsync(region);
            if (regionProps.IsError)
            {
                return Problem(regionProps.Errors);
            }
            var query = new GetFilteredProductsQuery(orderBy , filter ,
                minPrice , maxPrice , role , pageNum ,
                regionProps.Value.PoeRegion , regionProps.Value.ConstTax 
                );
            var result = await _mediator.Send(query);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Get product details by Id
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetProductById(
            Guid id,
            [FromQuery] string? region)
        {
            var regionProps = await _regionService.GetRegionPropsByNameAsync(region);
            if (regionProps.IsError)
            {
                return Problem(regionProps.Errors);
            }
            var query = new GetProductByIdQuery(id, regionProps.Value.PoeRegion, regionProps.Value.ConstTax);
            var result = await _mediator.Send(query);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }
    }
}

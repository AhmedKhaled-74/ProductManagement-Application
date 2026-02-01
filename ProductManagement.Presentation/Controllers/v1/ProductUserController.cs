using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using ProductManagement.Application.Commands.ProductCommands.User;
using ProductManagement.Application.DTOs.CategoryDTOs;
using ProductManagement.Application.DTOs.ProductDTOs;
using ProductManagement.Application.DTOs.ProductDTOs.AddRequest;
using ProductManagement.Application.DTOs.ProductDTOs.Result;
using ProductManagement.Application.DTOs.ProductDTOs.UpdateRequest;
using ProductManagement.Application.IServices;
using ProductManagement.Application.Queries.ProductQueries.Common;
using ProductManagement.Application.Queries.ProductQueries.User;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace ProductManagement.Presentation.Controllers.v1
{
    [Route("api/v{version:int}/Products")]
    [ApiVersion("1.0")]
    public class ProductUserController : CustomBaseController
    {
        private readonly ISender _mediator;
        private readonly IRegionService _regionService;
        private readonly IDistributedCache _distributedCache;

        public ProductUserController(ISender mediator, IRegionService regionService, IDistributedCache distributedCache)
        {
            _mediator = mediator;
            _regionService = regionService;
            _distributedCache = distributedCache;
        }

        /// <summary>
        /// Get filtered products with optional order, filter, and price range
        /// </summary>
        [HttpGet("filtered")]
        public async Task<IActionResult> GetFilteredProducts(
            [FromQuery] string? orderBy,
            [FromQuery] bool? isDescending,
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
            var query = new GetFilteredProductsQuery(
                orderBy,
                filter,
                minPrice,
                maxPrice,
                role,
                isDescending,
                pageNum,
                regionProps.Value.PoeRegion,
                regionProps.Value.ConstTax
            );
            var result = await _mediator.Send(query);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Get searched products by text
        /// </summary>
        [HttpGet("search/{searchFor}")]
        public async Task<IActionResult> SearchForProducts(
            [Required] string searchFor,
            [FromQuery] string? role,
            [FromQuery] int? pageNum,
            [FromQuery] string? region)
        {
            var regionProps = await _regionService.GetRegionPropsByNameAsync(region);
            if (regionProps.IsError)
            {
                return Problem(regionProps.Errors);
            }
            var query = new GetUserSearchProductsQuery(
                searchFor: searchFor,
                role: role,
                pageNum: pageNum,
                poeRegion: regionProps.Value.PoeRegion,
                cTax: regionProps.Value.ConstTax
            );
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get product details by Id
        /// </summary>
        [HttpGet("{productId:guid}")]
        public async Task<IActionResult> GetProductById(
            [Required] Guid productId,
            [FromQuery] string? region)
        {
            var regionProps = await _regionService.GetRegionPropsByNameAsync(region);
            if (regionProps.IsError)
            {
                return Problem(regionProps.Errors);
            }
            var query = new GetProductByIdQuery(productId, regionProps.Value.PoeRegion, regionProps.Value.ConstTax);
            var result = await _mediator.Send(query);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Add a product review
        /// </summary>
        [HttpPost("review")]
        public async Task<IActionResult> AddProductReview([FromBody] ProductReviewAddRequest addRequest)
        {
            var command = new AddProductReviewCommand(addRequest);
            var result = await _mediator.Send(command);
            if (result.IsError)
                return Problem(result.Errors);
            return Ok(result.Value);
        }

        /// <summary>
        /// Delete a product review
        /// </summary>
        [HttpDelete("review/{reviewId:guid}")]
        public async Task<IActionResult> DeleteProductReview(Guid reviewId, [FromQuery] Guid userId)
        {
            var command = new DeleteProductReviewCommand(reviewId, userId);
            var result = await _mediator.Send(command);
            if (result.IsError)
                return Problem(result.Errors);
            return Ok(result.Value);
        }

        /// <summary>
        /// Get a user's review for a specific product
        /// </summary>
        [HttpGet("{productId:guid}/review/user/{userId:guid}")]
        public async Task<IActionResult> GetUserReviewForProduct(Guid productId, Guid userId)
        {
            var query = new GetUserReviewForProductQuery(productId, userId);
            var result = await _mediator.Send(query);
            if (result.IsError)
                return Problem(result.Errors);
            return Ok(result.Value);
        }


        /// <summary>
        /// Search products for a user
        /// </summary>
        [HttpGet("search")]
        public async Task<IActionResult> GetUserSearchProducts(
            [FromQuery] string? searchFor,
            [FromQuery] string? role,
            [FromQuery] int? pageNum,
            [FromQuery] string? region)
        {
            var regionProps = await _regionService.GetRegionPropsByNameAsync(region);
            if (regionProps.IsError)
                return Problem(regionProps.Errors);
            var query = new GetUserSearchProductsQuery(
                searchFor,
                role,
                pageNum,
                regionProps.Value.PoeRegion,
                regionProps.Value.ConstTax
            );
            var result = await _mediator.Send(query);
            if (result.IsError)
                return Problem(result.Errors);
            return Ok(result.Value);
        }


        /// <summary>
        /// Get products with offers
        /// </summary>
        [HttpGet("offers")]
        public async Task<IActionResult> GetOffersProducts([FromQuery] string? region)
        {

            if (region == null)
                region = "Egypt";
            string cacheKey = $"products_offers:{region.ToLower()}";

            string? cachedProducts = await _distributedCache.GetStringAsync(cacheKey);
            if (cachedProducts != null)
            {

                var res = JsonSerializer.Deserialize<ProductsListResult<ProductResult>>(
                    cachedProducts,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
                return Ok(res);
            }

            var regionProps = await _regionService.GetRegionPropsByNameAsync(region);
            if (regionProps.IsError)
                return Problem(regionProps.Errors);
            var query = new GetOffersProductsQuery(
                regionProps.Value.PoeRegion,
                regionProps.Value.ConstTax
            );
            var result = await _mediator.Send(query);
            if (result.IsError)
                return Problem(result.Errors);

            string productsJson = JsonSerializer.Serialize(result.Value);
            var cacheOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(60))
                .SetSlidingExpiration(TimeSpan.FromSeconds(20));

            await _distributedCache.SetStringAsync(cacheKey, productsJson, cacheOptions);

            return Ok(result.Value);
        }


        /// <summary>
        /// method to get trended products
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        [HttpGet("trends")]
        public async Task<IActionResult> GetTrendedProducts([FromQuery] string? region)
        {
            if (region == null)
                region = "Egypt";

            string cacheKey = $"products_trends:{region.ToLower()}";

            // Try to get from cache (circuit breaker will handle failures)
            string? cachedProducts = await _distributedCache.GetStringAsync(cacheKey);

            if (cachedProducts != null)
            {
                var res = JsonSerializer.Deserialize<ProductsListResult<ProductResult>>(
                    cachedProducts,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
                return Ok(res);
            }

            // Cache miss or circuit open - get from database
            var regionProps = await _regionService.GetRegionPropsByNameAsync(region);
            if (regionProps.IsError)
                return Problem(regionProps.Errors);

            var query = new GetTrendsProductsQuery(
                regionProps.Value.PoeRegion,
                regionProps.Value.ConstTax
            );

            var result = await _mediator.Send(query);
            if (result.IsError)
                return Problem(result.Errors);

            // Try to cache (will fail silently if circuit is open)
            string productsJson = JsonSerializer.Serialize(result.Value);
            var cacheOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(60))
                .SetSlidingExpiration(TimeSpan.FromSeconds(20));

            await _distributedCache.SetStringAsync(cacheKey, productsJson, cacheOptions);

            return Ok(result.Value);
        }


        /// <summary>
        /// Update a product review
        /// </summary>
        [HttpPut("review")]
        public async Task<IActionResult> UpdateProductReview([FromBody] ProductReviewUpdateRequest updateRequest)
        {
            var command = new UpdateProductReviewCommand(updateRequest);
            var result = await _mediator.Send(command);
            if (result.IsError)
                return Problem(result.Errors);
            return Ok(result.Value);
        }


        /// <summary>
        /// method to get all product categories
        /// </summary>
        /// <returns></returns>
        [HttpGet("all-categories")]
        public async Task<IActionResult> GetAllCategories()
        {

            string cacheKey = $"products_categories";

            string? cachedCategories = await _distributedCache.GetStringAsync(cacheKey);

            if (cachedCategories != null)
            {
                var res = JsonSerializer.Deserialize<List<CategoryResultDTO>>(cachedCategories);
                return Ok(res);
            }
            var query = new GetAllCategoriesQuery();
            var result = await _mediator.Send(query);
            if (result.IsError)
                return Problem(result.Errors);
            string categoriesJson = JsonSerializer.Serialize(result.Value);
            var cacheOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(60))
                .SetSlidingExpiration(TimeSpan.FromSeconds(20));

            await _distributedCache.SetStringAsync(cacheKey, categoriesJson, cacheOptions);

            return Ok(result.Value);
        }
    }
}

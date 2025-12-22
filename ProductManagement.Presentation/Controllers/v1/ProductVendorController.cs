using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Application.Commands.ProductCommands.Vendor;
using ProductManagement.Application.Queries.ProductQueries.Vendor;
using ProductManagement.Application.DTOs.ProductDTOs.AddRequest;
using ProductManagement.Application.DTOs.ProductDTOs.UpdateRequest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Presentation.Controllers.v1
{
    [Route("api/v{version:int}/Products")]
    [ApiVersion("1.0")]

    public class ProductVendorController : CustomBaseController
    {
        private readonly ISender _mediator;

        public ProductVendorController(ISender mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all products for a vendor
        /// </summary>
        [HttpGet("vendor/{vendorId:guid}")]
        public async Task<IActionResult> GetVendorProducts(Guid vendorId, [FromQuery] int? pageNum)
        {
            var query = new GetVendorProductsQuery(vendorId, pageNum);
            var result = await _mediator.Send(query);
            if (result.IsError)
                return Problem(result.Errors);
            return Ok(result.Value);
        }

        /// <summary>
        /// Search vendor products
        /// </summary>
        [HttpGet("vendor/{vendorId:guid}/search")]
        public async Task<IActionResult> SearchVendorProducts(Guid vendorId, [FromQuery] string? searchTerm, [FromQuery] int? pageNum)
        {
            var query = new GetVendorSearchProductsQuery(vendorId, searchTerm, pageNum);
            var result = await _mediator.Send(query);
            if (result.IsError)
                return Problem(result.Errors);
            return Ok(result.Value);
        }

        /// <summary>
        /// Create new product
        /// </summary>
        [HttpPost("New-Product")]
        public async Task<IActionResult> CreateNewProduct([FromBody] ProductAddRequest productAddRequest)
        {
            var command = new CreateNewProductCommand(productAddRequest);
            var result = await _mediator.Send(command);
            if (result.IsError)
                return Problem(result.Errors);
            return Ok(new { message = "Added Successfully" });
        }

        /// <summary>
        /// Update product
        /// </summary>
        [HttpPut("update-product")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductUpdateRequest productUpdateRequest, [FromQuery] Guid userId)
        {
            var command = new UpdateProductCommand(productUpdateRequest, userId);
            var result = await _mediator.Send(command);
            if (result.IsError)
                return Problem(result.Errors);
            return Ok(new { message = "Updated Successfully" });
        }

        /// <summary>
        /// Delete product
        /// </summary>
        [HttpDelete("delete-product/{productId:guid}")]
        public async Task<IActionResult> DeleteProduct(Guid productId, [FromQuery] Guid userId)
        {
            var command = new DeleteProductCommand(productId, userId);
            var result = await _mediator.Send(command);
            if (result.IsError)
                return Problem(result.Errors);
            return Ok(new { message = "Deleted Successfully" });
        }

        /// <summary>
        /// Add product media
        /// </summary>
        [HttpPost("media")]
        public async Task<IActionResult> AddProductMedia([FromBody] ProductMediaAddRequest productMedia, [FromQuery] Guid userId)
        {
            var command = new AddProductMediaCommand(productMedia, userId);
            var result = await _mediator.Send(command);
            if (result.IsError)
                return Problem(result.Errors);
            return Ok(new { message = "Media Added Successfully" });
        }

        /// <summary>
        /// Update product media
        /// </summary>
        [HttpPut("media")]
        public async Task<IActionResult> UpdateProductMedia([FromBody] ProductMediaUpdateRequest productMediaUpdate, [FromQuery] Guid userId)
        {
            var command = new UpdateProductMediaCommand(productMediaUpdate, userId);
            var result = await _mediator.Send(command);
            if (result.IsError)
                return Problem(result.Errors);
            return Ok(new { message = "Media Updated Successfully" });
        }

        /// <summary>
        /// Delete product media
        /// </summary>
        [HttpDelete("media/{productMediaId:guid}")]
        public async Task<IActionResult> DeleteProductMedia(Guid productMediaId, [FromQuery] Guid userId)
        {
            var command = new DeleteProductMediaCommand(productMediaId, userId);
            var result = await _mediator.Send(command);
            if (result.IsError)
                return Problem(result.Errors);
            return Ok(new { message = "Media Deleted Successfully" });
        }

        /// <summary>
        /// Add product custom attribute
        /// </summary>
        [HttpPost("custom-attribute")]
        public async Task<IActionResult> AddProductCustomAttribute([FromBody] ProductCustomAttributeAddRequest productCustomAttribute, [FromQuery] Guid userId)
        {
            var command = new AddProductCustomAttributeCommand(productCustomAttribute, userId);
            var result = await _mediator.Send(command);
            if (result.IsError)
                return Problem(result.Errors);
            return Ok(new { message = "Custom Attribute Added Successfully" });
        }

        /// <summary>
        /// Update product custom attribute
        /// </summary>
        [HttpPut("custom-attribute")]
        public async Task<IActionResult> UpdateProductCustomAttribute([FromBody] ProductCustomAttributeUpdateRequest productCustomAttributeUpdate, [FromQuery] Guid userId)
        {
            var command = new UpdateProductCustomAttCommand(productCustomAttributeUpdate, userId);
            var result = await _mediator.Send(command);
            if (result.IsError)
                return Problem(result.Errors);
            return Ok(new { message = "Custom Attribute Updated Successfully" });
        }

        /// <summary>
        /// Delete product custom attribute
        /// </summary>
        [HttpDelete("custom-attribute/{productCustomAttributeId:guid}")]
        public async Task<IActionResult> DeleteProductCustomAttribute(Guid productCustomAttributeId, [FromQuery] Guid userId)
        {
            var command = new DeleteProductCustomAttributeCommand(productCustomAttributeId, userId);
            var result = await _mediator.Send(command);
            if (result.IsError)
                return Problem(result.Errors);
            return Ok(new { message = "Custom Attribute Deleted Successfully" });
        }

    }
}

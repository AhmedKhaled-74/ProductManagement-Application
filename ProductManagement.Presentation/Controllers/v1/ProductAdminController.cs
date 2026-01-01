using Asp.Versioning;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ProductManagement.Application.Commands.ProductCommands.Admin;
using ProductManagement.Application.Handlers.QueriesHandlers.ProductQueriesHandlers.Admin;
using ProductManagement.Application.Queries.ProductQueries.Admin;

namespace ProductManagement.Presentation.Controllers.v1
{
    [Route("api/v{version:int}/Products")]
    [ApiVersion("1.0")]
    public class ProductAdminController : CustomBaseController
    {
        private readonly ISender _mediator;

        public ProductAdminController(ISender mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// mark product as approved
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("{productId}/approve")]
        public async Task<IActionResult> ApproveProduct([FromRoute][Required] Guid productId, [FromQuery][Required] Guid userId)
        {
            var command = new ApproveProductCommand(productId, userId);
            var result = await _mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// admin delete product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("{productId}/admin")]
        public async Task<IActionResult> DeleteProduct([FromRoute][Required] Guid productId, [FromQuery][Required] Guid userId)
        {
            var command = new DeleteProductCommand(productId, userId);
            var result = await _mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// method to delete custom attribute by admin
        /// </summary>
        /// <param name="customAttributeId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("custom-attribute/{customAttributeId}/admin")]
        public async Task<IActionResult> DeleteCustomAttribute([FromRoute][Required] Guid customAttributeId, [FromQuery][Required] Guid userId)
        {
            var command = new DeleteProductCustomAttributeCommand(customAttributeId, userId);
            var result = await _mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// method to delete product media by admin
        /// </summary>
        /// <param name="productMediaId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("product-media/{productMediaId}/admin")]
        public async Task<IActionResult> DeleteProductMedia([FromRoute][Required] Guid productMediaId, [FromQuery][Required] Guid userId)
        {
            var command = new DeleteProductMediaCommand(productMediaId, userId);
            var result = await _mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// method to delete product review by admin
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("review/{reviewId}/admin")]
        public async Task<IActionResult> DeleteProductReview([FromRoute][Required] Guid reviewId, [FromQuery][Required] Guid userId)
        {
            var command = new DeleteProductReviewCommand(reviewId, userId);
            var result = await _mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// method to get unapproved products with pagination
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        [HttpGet("unapproved/{pageNum:int?}")]
        public async Task<IActionResult> GetUnapprovedProducts([FromQuery][Required] Guid userId, [FromQuery] int? pageNum)
        {
            var query = new GetUnapprovedProductsQuery(pageNum, userId);
            var result = await _mediator.Send(query);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result.Value);

        }
    }
}

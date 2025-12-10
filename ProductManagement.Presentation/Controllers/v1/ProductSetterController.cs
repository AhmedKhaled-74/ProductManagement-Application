using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Application.Commands.ProductCommands;
using ProductManagement.Application.DTOs.ProductDTOs.AddRequest;
using ProductManagement.Application.IServices;
using ProductManagement.Application.Queries.ProductQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Presentation.Controllers.v1
{
    [Route("api/v{version:int}/Products")]
    [ApiVersion("1.0")]

    public class ProductSetterController : CustomBaseController
    {
    
            private readonly ISender _mediator;

            public ProductSetterController(ISender mediator)
            {
                _mediator = mediator;
            }

            /// <summary>
            /// Create new product
            /// </summary>
            [HttpPost("New-Product")]
            public async Task<IActionResult> CreateNewProduct(ProductAddRequest productAddRequest)
            {
                var command = new CreateNewProductCommand(productAddRequest);
                var result = await _mediator.Send(command);
                if (result.IsError)
                {
                    return Problem(result.Errors);
                }

                return Ok(new { message = "Added Successfully" });
            }

        }
    }

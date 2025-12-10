using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Application.Errors;

namespace ProductManagement.Presentation.Controllers
{
    // Add this controller
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult HandleError() {
        var exeption =  HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

            if (exeption is NotFoundException ex)
                return Problem(title: exeption?.Message, statusCode: 400);
        return Problem(title:exeption?.Message); 
        }
    }

}
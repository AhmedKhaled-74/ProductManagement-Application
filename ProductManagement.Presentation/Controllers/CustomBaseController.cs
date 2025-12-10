using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace ProductManagement.Presentation.Controllers
{
    [ApiController]
    [Route("api/v{version:int}/[controller]")]
    public class CustomBaseController : ControllerBase
    {
        protected IActionResult Problem(List<Error> errors)
        {
            var e1 = errors[0];
            var statusCode = e1.Type switch
            {
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError,
            };
            return Problem(statusCode:statusCode , title:e1.Description);
        }
    }
}
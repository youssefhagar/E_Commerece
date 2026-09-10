using E_Commerece.Application.Common;
using E_Commerece.Domain.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerece.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiBaseController : ControllerBase
    {

        public static ActionResult<T> ToActionResult<T>(Result<T> result)
        {
            if (result.IsSuccess )
            {
                return new OkObjectResult(result);
            }
            else
            {
                return ToProblem(result.Errors);
            }
        }

        public static ActionResult ToActionResult(Result result)
        {
            if (result.IsSuccess)
            {
                return new OkResult();
            }
            else
            {
                return ToProblem(result.Errors);
            }
        }

        protected static ObjectResult ToProblem(IReadOnlyList<Error> errors)
        {
            var firstError = errors[0];
            var statusCode = firstError.ErrorType switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            };

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = firstError.Code,
                Detail = firstError.Description,
                Extensions =
                {
                    ["errors"] = errors.Select(e => new { e.Code, e.Description })
                }
            };
            return new ObjectResult(problemDetails);

        }


    }
}

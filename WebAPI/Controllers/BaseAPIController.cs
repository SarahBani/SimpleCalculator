using Common;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    public abstract class BaseAPIController : ControllerBase
    {

        #region Actions

        protected IActionResult GetSuccessfulResult<T>(T value)
        {
            return Ok(value);
        }

        protected IActionResult GetErrorResult(CustomException exception)
        {
            return Helper.GetBadRequestResult(exception.CustomMessage);
        }

        #endregion /Actions

    }
}

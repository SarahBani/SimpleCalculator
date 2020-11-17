using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace WebAPI.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //if (!context.ModelState.IsValid)
            //{
            //    context.Result = Helper.GetBadRequestResult(context.ModelState);
            //}
            if (context.ActionArguments.Count == 0)
            {
                context.Result = Helper.GetBadRequestResult(Constant.Exception_NullInput);
                return;
            }
            var numbers = context.ActionArguments["numbers"] as decimal[];
            if (numbers == null || numbers.Length == 0)
            {
                context.Result = Helper.GetBadRequestResult(Constant.Exception_EmptyArray);
            }
        }

    }
}

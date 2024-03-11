using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PixelStacker.Web.Net.AppStart;
using PixelStacker.Web.Net.Models.Attributes;

namespace PixelStacker.Web.Net.Controllers
{
    [RequestTimeFilter]
    [ModelStateValidationFilter]
    [JsonPayloadFilter]
    [Route("api/[controller]/[action]")]
    public class BaseApiController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Do a bunch of stuff here if needed. Stuff like validation.
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            // Make sure HTML helpers get values from MODEL and not modelstate. (Except for validation errors)
            ModelState.ToList().Select(x => x.Value).ToList().ForEach(x => { x.AttemptedValue = null; x.RawValue = null; });

            // Do a bunch of stuff here if needed. Stuff like validation.
            base.OnActionExecuted(context);
        }
    }
}
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace PixelStacker.Web.Net.Controllers
{
    public class TestController : BaseApiController
    {
        [HttpGet]
        public JsonResult Headers()
        {
            var headers = Request.Headers.ToList();
            return new JsonResult(headers);
        }
    }
}

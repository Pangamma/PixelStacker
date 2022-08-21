using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace PixelStacker.Web.Net.AppStart
{
    public class CorsHandler : BaseHandlerMiddleware
    {
        public CorsHandler(RequestDelegate next) : base(next) { }
        public override async Task Invoke(HttpContext context)
        {
            await base.Invoke(context);
        }

        public override async Task BeforeInvoke(HttpContext context)
        {
            if (!context.Response.HasStarted)
            {
                var h = context.Response.Headers;
                h["Access-Control-Allow-Origin"] = "*";
                h["Access-Control-Allow-Methods"] = "*";
                h["Access-Control-Allow-Headers"] = "*";
            }

            await base.BeforeInvoke(context);
        }
    }
}

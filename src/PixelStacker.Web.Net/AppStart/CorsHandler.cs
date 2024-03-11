using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace PixelStacker.Web.Net.AppStart
{
    public class CorsHandler : BaseHandlerMiddleware
    {
        public override Task BeforeInvoke(HttpContext context)
        {
            if (!context.Response.HasStarted)
            {
                var h = context.Response.Headers;
                h["Access-Control-Allow-Origin"] = "*";
                h["Access-Control-Allow-Methods"] = "*";
                h["Access-Control-Allow-Headers"] = "*";
            }

            return base.BeforeInvoke(context);
        }
    }
}

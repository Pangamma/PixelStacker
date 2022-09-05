using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace PixelStacker.Web.Net.Utility
{
    public static class HttpExtensions
    {
        public static IApplicationBuilder RedirectWhen(this IApplicationBuilder app, Func<string, bool> pathMatchFunc, Func<HttpContext, string> dstPathFunc)
        {
            // API Index should be swagger
            app.UseWhen((context) => pathMatchFunc(context.Request.Path.Value), configWhen =>
            {
                configWhen.Run((HttpContext x) =>
                {
                    string redirectUrl = dstPathFunc(x);
                    // (x.Request.Host.Host == "taylorlove.info") ? "/projects/pixelstacker/demo" : "/demo";
                    x.Response.Redirect(redirectUrl);
                    return Task.CompletedTask;
                });
            });

            return app;
        }
    }
}

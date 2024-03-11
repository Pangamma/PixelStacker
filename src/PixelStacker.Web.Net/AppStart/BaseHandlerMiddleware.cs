using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace PixelStacker.Web.Net.AppStart
{
    public class BaseHandlerMiddleware: IMiddleware
    {
        public virtual Task BeforeInvoke(HttpContext context) => Task.CompletedTask;
        public virtual Task AfterInvoke(HttpContext context) => Task.CompletedTask;

        public virtual async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await this.BeforeInvoke(context);
            await next(context);
            await this.AfterInvoke(context);
        }
    }
}

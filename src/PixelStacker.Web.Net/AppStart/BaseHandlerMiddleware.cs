using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace PixelStacker.Web.Net.AppStart
{
    public class BaseHandlerMiddleware
    {
        protected RequestDelegate _next { get; set; }

        public BaseHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public virtual Task BeforeInvoke(HttpContext context) => Task.CompletedTask;
        public virtual Task AfterInvoke(HttpContext context) => Task.CompletedTask;

        public virtual async Task Invoke(HttpContext context)
        {
            await this.BeforeInvoke(context);
            await _next(context);
            await this.AfterInvoke(context);
        }
    }
}

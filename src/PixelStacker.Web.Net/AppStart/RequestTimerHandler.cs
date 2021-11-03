using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PixelStacker.Web.Net.AppStart
{
    public class RequestTimerHandler
    {
        protected RequestDelegate _next { get; set; }

        public RequestTimerHandler(RequestDelegate next)
        {
            _next = next;
        }


        public async Task Invoke(HttpContext context)
        {
            // Start a timer...  Stop the timer when all other handlers have been completed.
            var start = DateTimeOffset.UtcNow;
            var watcher = Stopwatch.StartNew();
            await _next(context);
            watcher.Stop();
            context.Response.Headers.TryAdd("X-RequestTimeMs", watcher.ElapsedMilliseconds.ToString());
        }
    }

    public class RequestTimeFilter: ActionFilterAttribute
    {
        private const string ResponseTimeKey = "X-ResponseTimeMs";
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Start the timer   
            context.HttpContext.Items[ResponseTimeKey] = Stopwatch.StartNew();
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Stopwatch stopwatch = (Stopwatch) context.HttpContext.Items[ResponseTimeKey];
            // Calculate the time elapsed   
            var timeElapsed = stopwatch.ElapsedMilliseconds;
            stopwatch.Stop();
            context.HttpContext.Response.Headers.Add(ResponseTimeKey, timeElapsed.ToString());
            base.OnActionExecuted(context);
        }
    }
}

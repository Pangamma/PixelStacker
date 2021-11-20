using Microsoft.AspNetCore.Http;
using PixelStacker.Web.Net.Models;
using System;
using System.Net;
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

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.BeforeInvoke(context);
                await _next(context);
                await this.AfterInvoke(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int) HttpStatusCode.InternalServerError;

                var result = error?.Message;

                string json = new JsonPayload<object>(error, result)
                {
                    StatusCode = ResponseStatusCode.INTERNAL_SERVER_ERROR
                }.ToJsonString();

                await response.WriteAsync(json);
            }
        }
    }
}

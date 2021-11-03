using Microsoft.AspNetCore.Http;
using MtCoffee.Web.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace PixelStacker.Web.Net.AppStart
{
    public class ErrorHandler : BaseHandlerMiddleware
    {
        public ErrorHandler(RequestDelegate next) : base(next) { }
        public override Task BeforeInvoke(HttpContext context) => Task.CompletedTask;

        public override async Task AfterInvoke(HttpContext context)
        {
            int codeCat = context.Response.StatusCode / 100;
            if (codeCat == 2 || codeCat == 3) { return; }
            if (context.Items.ContainsKey("x-JsonPayload")) { return; }

            ResponseStatusCode code;
            switch (context.Response.StatusCode)
            {
                case (int) HttpStatusCode.BadRequest:
                    code = ResponseStatusCode.INVALID_ARGUMENTS;
                    break;
                case (int) HttpStatusCode.InternalServerError:
                    code = ResponseStatusCode.INTERNAL_SERVER_ERROR;
                    break;
                case (int) HttpStatusCode.NotFound:
                    code = ResponseStatusCode.NOT_FOUND;
                    break;
                case (int) HttpStatusCode.Forbidden:
                    code = ResponseStatusCode.FORBIDDEN;
                    break;
                case (int) HttpStatusCode.Unauthorized:
                    code = ResponseStatusCode.NOT_AUTHORIZED;
                    break;
                default:
                    code = ResponseStatusCode.NON_SUCCESS;
                    break;
            }

            context.Response.Clear();
            await context.Response.WriteAsync(new JsonPayload<object>() { 
                StatusCode = code,
                Errors = new List<string>() { code.Description }
            }.ToJsonString());
        }

    }
}

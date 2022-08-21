using Microsoft.AspNetCore.Http;
using PixelStacker.Web.Net.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace PixelStacker.Web.Net.AppStart
{
    public class ErrorHandler : BaseHandlerMiddleware
    {
        public ErrorHandler(RequestDelegate next) : base(next) { }
        public override Task BeforeInvoke(HttpContext context) => Task.CompletedTask;

        public override async Task Invoke(HttpContext context)
        {
            try
            {
                await base.Invoke(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                var payloadCode = GetResponseStatusCode((HttpStatusCode)response.StatusCode);
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var result = error?.Message;

                string json = new JsonPayload<object>(error, result)
                {
                    StatusCode = payloadCode
                }.ToJsonString();
                await response.WriteAsync(json);
            }
        }

        public ResponseStatusCode GetResponseStatusCode(HttpStatusCode statCode)
        {
            ResponseStatusCode code;
            switch (statCode)
            {
                case HttpStatusCode.BadRequest:
                    code = ResponseStatusCode.INVALID_ARGUMENTS;
                    break;
                case HttpStatusCode.InternalServerError:
                    code = ResponseStatusCode.INTERNAL_SERVER_ERROR;
                    break;
                case HttpStatusCode.NotFound:
                    code = ResponseStatusCode.NOT_FOUND;
                    break;
                case HttpStatusCode.Forbidden:
                    code = ResponseStatusCode.FORBIDDEN;
                    break;
                case HttpStatusCode.Unauthorized:
                    code = ResponseStatusCode.NOT_AUTHORIZED;
                    break;
                case HttpStatusCode.Accepted:
                case HttpStatusCode.OK:
                case HttpStatusCode.Created:
                    code = ResponseStatusCode.SUCCESS;
                    break;
                default:
                    code = ResponseStatusCode.NON_SUCCESS;
                    break;
            }

            return code;
        }

        public override async Task AfterInvoke(HttpContext context)
        {
            int codeCat = context.Response.StatusCode / 100;
            if (codeCat == 2 || codeCat == 3) { return; }
            if (context.Items.ContainsKey("x-JsonPayload")) { return; }
            if (context.Response.HasStarted) { return; }
            var statCode = context.Response.StatusCode;
            var payloadCode = GetResponseStatusCode((HttpStatusCode)context.Response.StatusCode);

            //context.Response.Clear();
            context.Response.StatusCode = statCode;
            await context.Response.WriteAsync(new JsonPayload<object>()
            {
                StatusCode = payloadCode,
                Errors = new List<string>() { payloadCode.Description }
            }.ToJsonString());
        }

    }
}

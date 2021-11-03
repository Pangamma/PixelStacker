using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MtCoffee.Web.AppStart
{
    public class SwaggerConfig
    {
        public static void AddSwaggerGen(SwaggerGenOptions c)
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "PixelStacker WEB", Version = "v1" });
            c.ResolveConflictingActions(apiDesc =>
            {
                return apiDesc.First();
            });

            c.IgnoreObsoleteActions();
            c.IgnoreObsoleteProperties();
        }

        public static void UseSwaggerUI(SwaggerUIOptions c)
        {
            c.SwaggerEndpoint("./v1/swagger.json", "v1");
            c.ShowCommonExtensions();
            c.DocExpansion(DocExpansion.None);
            c.DocumentTitle = "PixelStacker WEB";
            #if !DEBUG
            c.RoutePrefix = "";
            #endif

        }

        public static void UseSwagger(SwaggerOptions c)
        {
            c.PreSerializeFilters.Add((swagger, httpReq) =>
            {
                if (httpReq.Headers.ContainsKey("Host"))
                {
                    //The httpReq.PathBase and httpReq.Headers["X-Forwarded-Prefix"] is what we need to get the base path.
                    //For some reason, they are returning as null/blank. Perhaps this has something to do with how the proxy is configured which I don't have control.
                    //For the time being, the base path is manually set here that corresponds to the APIM API Url Prefix.
                    //In this case we set it to 'sample-app'. 

                    var basePath = httpReq.Headers["Host"] == "taylorlove.info" ? "projects/pixelstacker" : "";
                    var serverUrl = $"https://{httpReq.Headers["Host"]}/{basePath}";
                    Debug.WriteLine(serverUrl);
                    swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = serverUrl } };
                }
            });
        }
    }
}

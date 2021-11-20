using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using PixelStacker.Web.Net.Models.Attributes;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PixelStacker.Web.Net.AppStart
{
    public class SwaggerConfig
    {
        public static void AddSwaggerGen(SwaggerGenOptions c)
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "PixelStacker WEB", Version = "v1" });
            c.ResolveConflictingActions(apiDesc =>
            {
                return apiDesc.First();
            });

            c.IgnoreObsoleteActions();
            c.IgnoreObsoleteProperties();
            c.ParameterFilter<AcceptableValuesFilter>();
        }

        public static void UseSwaggerUI(SwaggerUIOptions c)
        {
            c.SwaggerEndpoint("./v1/swagger.json", "v1");
            c.ShowCommonExtensions();
            c.DocExpansion(DocExpansion.None);
            c.DocumentTitle = "PixelStacker WEB";
            c.InjectStylesheet("../swagger-ui/custom-swagger.css");

//#if !DEBUG
//            c.RoutePrefix = "";
//#endif

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

    public class AcceptableValuesFilter : IParameterFilter
    {
        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            var meta = context.ApiParameterDescription.ModelMetadata as DefaultModelMetadata;
            if (meta is null) return;
            var propAttrs = meta.Attributes.PropertyAttributes;
            if (propAttrs is null) return;

            foreach (var attr in propAttrs.OfType<AcceptableIntValuesAttribute>())
            {
                parameter.Schema.Enum.Clear();
                foreach (var val in attr.AllowableValues)
                {
                    parameter.Schema.Enum.Add(new OpenApiInteger(val));
                }
            }

            foreach (var attr in propAttrs.OfType<AcceptableStringValuesAttribute>())
            {
                parameter.Schema.Enum.Clear();
                foreach (var val in attr.AllowableValues)
                {
                    parameter.Schema.Enum.Add(new OpenApiString(val));
                }
            }
        }
    }

    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema model, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                model.Enum.Clear();
                Enum.GetNames(context.Type)
                    .ToList()
                    .ForEach(n => model.Enum.Add(new OpenApiString(n)));
            }
        }
    }
}

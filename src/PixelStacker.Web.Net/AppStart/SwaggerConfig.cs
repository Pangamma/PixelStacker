using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using PixelStacker.Web.Net.Models.Attributes;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Nodes;

namespace PixelStacker.Web.Net.AppStart
{
    public class SwaggerConfig
    {
        public static void AddSwaggerGen(SwaggerGenOptions c)
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "PixelStacker WEB",
                Version = "v1",
                Contact = new OpenApiContact()
                {
                    Url = new Uri("https://github.com/Pangamma/PixelStacker"),
                    Name = "View Source"
                }


            });
            c.ResolveConflictingActions(apiDesc =>
            {
                return apiDesc.First();
            });

            c.IgnoreObsoleteActions();
            c.IgnoreObsoleteProperties();
            c.ParameterFilter<AcceptableValuesFilter>();
            c.ParameterFilter<DefaultValueFilter>();
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


    public class DefaultValueFilter : IParameterFilter
    {
        // parameter.Schema can be a concrete OpenApiSchema or an OpenApiSchemaReference ($ref wrapper);
        // Example is only settable on the concrete schema, so references need to go through RecursiveTarget.
        private static void SetExample(IOpenApiSchema schema, JsonNode example)
        {
            if (schema is OpenApiSchema concrete)
            {
                concrete.Example = example;
            }
            else if (schema is OpenApiSchemaReference reference && reference.RecursiveTarget is OpenApiSchema target)
            {
                target.Example = example;
            }
        }

        public void Apply(IOpenApiParameter parameter, ParameterFilterContext context)
        {
            var meta = context.ApiParameterDescription.ModelMetadata as DefaultModelMetadata;
            if (meta is null) return;
            var propAttrs = meta.Attributes.PropertyAttributes;
            if (propAttrs is null) return;

            foreach (var attr in propAttrs.OfType<DefaultValueAttribute>())
            {
                if (attr.Value != null)
                {
                    SetExample(parameter.Schema, JsonNode.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(attr.Value)));
                    return;
                }
            }

            var parentModelType = context.ParameterInfo?.ParameterType;
            if (parentModelType != null)
            {
                object parentModel;
                try
                {
                    parentModel = Activator.CreateInstance(parentModelType);
                }
                catch
                {
                    parentModel = null;
                }

                if (parentModel != null)
                {
                    PropertyDescriptor prop = TypeDescriptor.GetProperties(parentModel)[parameter.Name];
                    if (prop.CanResetValue(parentModel)) prop.ResetValue(parentModel);
                    var defVal = prop.GetValue(parentModel);
                    if (defVal != null)
                    {
                        string json = !defVal.GetType().IsEnum
                        ? Newtonsoft.Json.JsonConvert.SerializeObject(defVal)
                        : Newtonsoft.Json.JsonConvert.SerializeObject(defVal.ToString());
                        SetExample(parameter.Schema, JsonNode.Parse(json));
                    }
                }
            }
        }
    }

    public class AcceptableValuesFilter : IParameterFilter
    {
        // Same reference-vs-concrete issue as DefaultValueFilter.SetExample: resolve to the concrete
        // schema first, and Enum may not be pre-initialized on it.
        private static void SetEnum(IOpenApiSchema schema, IEnumerable<JsonNode> values)
        {
            var concrete = schema as OpenApiSchema ?? (schema as OpenApiSchemaReference)?.RecursiveTarget;
            if (concrete is null) return;

            concrete.Enum ??= new List<JsonNode>();
            concrete.Enum.Clear();
            foreach (var value in values)
            {
                concrete.Enum.Add(value);
            }
        }

        public void Apply(IOpenApiParameter parameter, ParameterFilterContext context)
        {
            var meta = context.ApiParameterDescription.ModelMetadata as DefaultModelMetadata;
            if (meta is null) return;
            var propAttrs = meta.Attributes.PropertyAttributes;
            if (propAttrs is null) return;

            foreach (var attr in propAttrs.OfType<AcceptableIntValuesAttribute>())
            {
                SetEnum(parameter.Schema, attr.AllowableValues.Select(val => (JsonNode)JsonValue.Create(val)));
            }

            foreach (var attr in propAttrs.OfType<AcceptableStringValuesAttribute>())
            {
                SetEnum(parameter.Schema, attr.AllowableValues.Select(val => (JsonNode)JsonValue.Create(val)));
            }
        }
    }

    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(IOpenApiSchema model, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                model.Enum.Clear();
                Enum.GetNames(context.Type)
                    .ToList()
                    .ForEach(n => model.Enum.Add(JsonValue.Create(n)));
            }
        }
    }
}

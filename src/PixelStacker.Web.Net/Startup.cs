using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using PixelStacker.Web.Net.AppStart;
using PixelStacker.Web.Net.Controllers;
using PixelStacker.Web.Net.Utility;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PixelStacker.Web.Net
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddSwaggerGen(SwaggerConfig.AddSwaggerGen);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            DeploymentInfoController.ExtraProperties["Environment"] = env;
            // https://stackoverflow.com/a/43878365/1582837
            var forwardedHeadersOptions = new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto, RequireHeaderSymmetry = false };
            forwardedHeadersOptions.KnownNetworks.Clear();
            forwardedHeadersOptions.KnownProxies.Clear();

            app
                .UseForwardedHeaders()
                .UseHttpsRedirection()
                .UseMiddleware<CorsHandler>()
                .UseMiddleware<ErrorHandler>()
                .UseStaticFiles()   // Add static files  BEFORE adding routing calls.
            ;

            string pathFolder = env.IsProduction() ? "/projects/pixelstacker" : "";

#if API_INCLUDES_DEMO
            // Redirects for the SPA
            {
                // Root index should be the demo
                app.RedirectWhen(path => path == "/", x => $"{pathFolder}/demo/");
                app.RedirectWhen(path => path == "/demo", x => $"{pathFolder}/demo/");
                var fp = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "wwwroot", "dist"));
                var fsOpts = new FileServerOptions
                {
                    FileProvider = fp,
                    RequestPath = "/demo", // Anything after /demo will try and obtain data from wwwroot/dist.
                    RedirectToAppendTrailingSlash = false,
                };

                fsOpts.EnableDefaultFiles = true;
                fsOpts.DefaultFilesOptions.DefaultFileNames = new string[] { $"index.html" };
                app.UseFileServer(fsOpts);
            }
#else
            app.RedirectWhen(path => path == "/", x => $"{pathFolder}/swagger/index.html");
#endif

            // API Index should be swagger
            app.RedirectWhen(path => path == "/api", x => $"{pathFolder}/swagger/index.html");

            // Handle API routing
            app.UseWhen((context) => context.Request.Path.Value.ToLower().StartsWith("/api"),
                configWhen => configWhen.UseMvc((Microsoft.AspNetCore.Routing.IRouteBuilder routes) =>
                {
                    routes.MapRoute(
                        name: "specific-api-route",
                        template: "api/{controller}/{action?}/{id?}",
                        defaults: new { controller = "DeploymentInfo", action = "Index" }
                    );

                    routes.MapRoute(
                        name: "default-api-route",
                        template: "api",
                        defaults: new { controller = "DeploymentInfo", action = "Index" }
                    );
                }
            ));


            app.UseSwagger(SwaggerConfig.UseSwagger);
            app.UseSwaggerUI(SwaggerConfig.UseSwaggerUI);
        }
    }
}

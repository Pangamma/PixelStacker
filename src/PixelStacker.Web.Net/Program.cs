using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using PixelStacker.Web.Net;
using PixelStacker.Web.Net.Controllers;
using System;

Console.WriteLine($"Starting app at {DeploymentInfoController.DeploymentTime.Value}");
Host.CreateDefaultBuilder(args)
    .UseSystemd()
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        string envName = hostingContext.HostingEnvironment.EnvironmentName;
        if (envName == "Development")
        {
            config.AddJsonFile("appsettings.Development.json",
                optional: false,
                reloadOnChange: true);
        }
        else
        {
            config.AddJsonFile("appsettings.json",
                optional: false,
                reloadOnChange: true); ;
        }
    })
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>()
        .PreferHostingUrls(true)
        .UseUrls(new string[] { "http://127.0.0.1:5005"
#if DEBUG
            , "https://127.0.0.1:53739"
#endif
        });
    })
    .Build()
    .Run();

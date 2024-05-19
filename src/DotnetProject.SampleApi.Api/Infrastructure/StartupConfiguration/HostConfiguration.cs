

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace DotnetProject.SampleApi.Api.Infrastructure.StartupConfiguration
{
    public static class HostConfiguration
    {
        public static WebApplicationBuilder ConfigureHost(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile("appsecrets.json", optional: true, reloadOnChange: false);

            builder.Host.UseSerilog((hostingContext, services, loggerConfiguration) => loggerConfiguration
                .ReadFrom.Configuration(hostingContext.Configuration)
                .Enrich.FromLogContext());

            return builder;
        }
    }
}

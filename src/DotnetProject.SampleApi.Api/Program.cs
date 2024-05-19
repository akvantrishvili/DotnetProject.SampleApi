using System;
using System.Globalization;
using DotnetProject.SampleApi.Api.Infrastructure.StartupConfiguration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
    .CreateBootstrapLogger();

try
{
    
    Log.Information("Starting host");

    var builder = WebApplication.CreateBuilder(args);

    Log.Information("Configuring web host");
    builder.ConfigureHost();

    Log.Information("Configuring services");
    builder.ConfigureServices();

    var app = builder.Build();

    Log.Information("Configuring middleware");
    app.ConfigureMiddleware();

    Log.Information("Starting app");
    app.Run();
    Log.Information("Stopping host");
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

return 0;


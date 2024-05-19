using Amazon.Runtime.Internal;
using DotnetProject.SampleApi.Persistence.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace DotnetProject.SampleApi.Api.Infrastructure.StartupConfiguration
{
    public static class MiddlewareConfiguration
    {
        public static WebApplication ConfigureMiddleware(this WebApplication app)
        {
            // Set request path base in case API is published under some path (e.g. virtual directory) instead of root path.
            // If API is published in IIS virtual directory, then ASPNETCORE_APPL_PATH env variable is automatically set.
            // If API is published behind reverse proxy, under some path, then ASPNETCORE_APPL_PATH env variable or appsettings must be set manually.
            var basePath = app.Configuration.GetValue<string>("ASPNETCORE_APPL_PATH")?.Trim('/').ToLower();
            if (!string.IsNullOrWhiteSpace(basePath))
                app.UsePathBase($"/{basePath}");

            using var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.Migrate();

            // See details here: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization
            app.UseRequestLocalization();

            // Register exception handler middleware with error code descriptions.
            // See details here: https://confluence.tbcbank.ge/display/SD/API+Error+Handling
            //app.UseApiErrorHandling()
            //    .AddExceptionHandler<ExceptionHandler>()
            //    .AddErrorCodeDescriptions(DomainErrorCodes.GetErrorCodeDescriptions)
            //    .AddErrorCodeDescriptions(ApplicationErrorCodes.GetErrorCodeDescriptions);

            app.UseVersionedSwagger();

            // Add API related security headers.
            // See details here: https://cheatsheetseries.owasp.org/cheatsheets/REST_Security_Cheat_Sheet.html#security-headers
            app.AddApiSecurityHeaders();

            app.UseHttpsRedirection();

            app.UseRouting();

            // CORS middleware must be configured to execute between the calls to UseRouting and UseEndpoints.
            // See details here: https://docs.microsoft.com/en-us/aspnet/core/security/cors
            app.UseCors();


            app.MapHealthChecks("/health").AllowAnonymous();

            app.MapControllers();

            return app;
        }
    }
}

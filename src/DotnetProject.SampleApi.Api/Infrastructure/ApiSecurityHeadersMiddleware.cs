

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;

namespace DotnetProject.SampleApi.Api.Infrastructure
{
    // https://cheatsheetseries.owasp.org/cheatsheets/REST_Security_Cheat_Sheet.html#security-headers
    public class ApiSecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiSecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context, IWebHostEnvironment env)
        {
            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Referrer-Policy
            // Non-HTML responses should not trigger additional requests.
            context.Response.Headers.TryAdd("Referrer-Policy", new StringValues("no-referrer"));

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Content-Type-Options
            // To prevent browsers from performing MIME sniffing, and inappropriately interpreting responses as HTML.
            context.Response.Headers.TryAdd("X-Content-Type-Options", new StringValues("nosniff"));

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Frame-Options
            // To protect against drag-and-drop style clickjacking attacks.
            context.Response.Headers.TryAdd("X-Frame-Options", new StringValues("DENY"));

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/CSP
            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy
            // The majority of CSP functionality only affects pages rendered as HTML.
            var csp = "default-src 'none'";
            context.Response.Headers.TryAdd("Content-Security-Policy", new StringValues(csp));
            // for IE
            context.Response.Headers.TryAdd("X-Content-Security-Policy", new StringValues(csp));

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Feature-Policy
            // Feature policies only affect pages rendered as HTML.
            context.Response.Headers.TryAdd("Feature-Policy", new StringValues("'none'"));

            return _next(context);
        }
    }

    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder AddApiSecurityHeaders(this IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();

            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseMiddleware<ApiSecurityHeadersMiddleware>();

            return app;
        }
    }
}

// Copyright (C) TBC Bank. All Rights Reserved.

using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace DotnetProject.SampleApi.Api.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseVersionedSwagger(this IApplicationBuilder app)
        {
            return app.UseVersionedSwagger(null, null);
        }

        public static IApplicationBuilder UseVersionedSwagger(this IApplicationBuilder app,
            Action<SwaggerUIOptions>? swaggerUiOptions,
            Action<SwaggerOptions>? swaggerOptions)
        {
            app.UseSwagger(options =>
            {
                options.RouteTemplate = "swagger/{documentName}/swagger.json";
                swaggerOptions?.Invoke(options);
            });

            var versionDescriptionProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

            app.UseSwaggerUI(options =>
            {
                foreach (var description in versionDescriptionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
                swaggerUiOptions?.Invoke(options);
            });

            return app;
        }
    }
}

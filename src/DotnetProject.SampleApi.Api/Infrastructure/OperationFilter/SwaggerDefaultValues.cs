// Copyright (C) TBC Bank. All Rights Reserved.

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace DotnetProject.SampleApi.Api.Infrastructure.OperationFilter
{
    [RequiresUnreferencedCode("Uses JsonSerializer to serialize default values")]
    public class SwaggerDefaultValues : IOperationFilter
    {
        /// <summary>
        /// Applies the filter to the specified operation using the given context.
        /// </summary>
        /// <param name="operation">The operation to apply the filter to.</param>
        /// <param name="context">The current operation filter context.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;

            operation.Deprecated |= apiDescription.IsDeprecated();

            // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/1752#issue-663991077
            foreach (var responseType in context.ApiDescription.SupportedResponseTypes)
            {
                // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/b7cf75e7905050305b115dd96640ddd6e74c7ac9/src/Swashbuckle.AspNetCore.SwaggerGen/SwaggerGenerator/SwaggerGenerator.cs#L383-L387
                var responseKey = responseType.IsDefaultResponse
                    ? "default"
                    : responseType.StatusCode.ToString(CultureInfo.InvariantCulture);
                var response = operation.Responses[responseKey];

                foreach (var contentType in response.Content.Keys)
                {
                    if (!responseType.ApiResponseFormats.Any(x => x.MediaType == contentType))
                        response.Content.Remove(contentType);
                }
            }

            if (operation.Parameters == null)
                return;

            // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/412
            // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/pull/413
            foreach (var parameter in operation.Parameters)
            {
                var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

                parameter.Description ??= description.ModelMetadata?.Description;

                // Default value is not guaranteed to always be a string
                if (parameter.Schema.Default == null &&
                    description.DefaultValue != null &&
                    description.DefaultValue is not DBNull &&
                    description.ModelMetadata is ModelMetadata modelMetadata)
                {
                    // REF: https://github.com/dotnet/aspnet-api-versioning/issues/429#issuecomment-605402330
                    var json = JsonSerializer.Serialize(description.DefaultValue, modelMetadata.ModelType);
                    parameter.Schema.Default = OpenApiAnyFactory.CreateFromJson(json);
                }

                parameter.Required |= description.IsRequired;
            }
        }
    }
}

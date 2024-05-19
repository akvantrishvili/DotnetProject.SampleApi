// Copyright (C) TBC Bank. All Rights Reserved.

using System.Collections.Generic;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;


namespace DotnetProject.SampleApi.Api.Infrastructure.OperationFilter
{
    public static class SwaggerGenOptionsExtensions
    {
        public static SwaggerGenOptions AddErrorCodeDescriptions(
            this SwaggerGenOptions options,
            bool generateExamples = true,
            IEnumerable<Assembly>? errorCodeExamplesAssembly = null)
        {
            //errorCodeExamplesAssembly ??= (IEnumerable<Assembly>)new List<Assembly>()
            //{
            //    Assembly.GetCallingAssembly()
            //};
            //options.OperationFilter<ErrorCodeOperationFilter>((object)generateExamples, (object)errorCodeExamplesAssembly);
            //options.SchemaFilter<NoAdditionalProperties<ApiProblemDetails>>();
            return options;
        }

        public static SwaggerGenOptions AddErrorCodeDescriptions<TApiProblemDetails>(
            this SwaggerGenOptions options,
            bool generateExamples = true,
            IEnumerable<Assembly>? errorCodeExamplesAssembly = null)
            //where TApiProblemDetails : ApiProblemDetails, new()
        {
            errorCodeExamplesAssembly ??= (IEnumerable<Assembly>)new List<Assembly>()
            {
                Assembly.GetCallingAssembly()
            };
            //options.OperationFilter<ErrorCodeOperationFilter<TApiProblemDetails>>((object)generateExamples, (object)errorCodeExamplesAssembly);
            //options.SchemaFilter<NoAdditionalProperties<TApiProblemDetails>>();
            return options;
        }
    }
}

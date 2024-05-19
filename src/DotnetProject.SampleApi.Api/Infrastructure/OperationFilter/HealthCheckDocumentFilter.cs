
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DotnetProject.SampleApi.Api.Infrastructure.OperationFilter
{
    public class HealthCheckDocumentFilter : IDocumentFilter
    {
        private readonly string _routePath;
        private readonly IDictionary<HealthStatus, int>? _resultStatusCodes;

        public HealthCheckDocumentFilter(
          string routePath,
          IDictionary<HealthStatus, int>? resultStatusCodes = null)
        {
            _routePath = !string.IsNullOrEmpty(routePath) ? routePath : throw new ArgumentNullException(nameof(routePath));
            IDictionary<HealthStatus, int>? dictionary;
            if (resultStatusCodes == null || resultStatusCodes.Count <= 0)
                dictionary = (IDictionary<HealthStatus, int>)new Dictionary<HealthStatus, int>()
        {
          {
            HealthStatus.Healthy,
            200
          },
          {
            HealthStatus.Degraded,
            200
          },
          {
            HealthStatus.Unhealthy,
            503
          }
        };
            else
                dictionary = resultStatusCodes;
            _resultStatusCodes = dictionary;
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var openApiPathItem = new OpenApiPathItem();
            var operation = new OpenApiOperation();
            operation.Summary = "Service health check";
            operation.Description = _resultStatusCodes != null ? "Check if service is " + string.Join<HealthStatus>(", ", (IEnumerable<HealthStatus>)_resultStatusCodes.Keys) : "";
            operation.Tags.Add(new OpenApiTag()
            {
                Name = "Health Check"
            });
            if (_resultStatusCodes != null)
            {
                foreach (var keyValuePair in _resultStatusCodes.GroupBy<KeyValuePair<HealthStatus, int>, int>((Func<KeyValuePair<HealthStatus, int>, int>)(x => x.Value)).ToDictionary<IGrouping<int, KeyValuePair<HealthStatus, int>>, string, IEnumerable<string>>((Func<IGrouping<int, KeyValuePair<HealthStatus, int>>, string>)(x => x.Key.ToString()), (Func<IGrouping<int, KeyValuePair<HealthStatus, int>>, IEnumerable<string>>)(x => x.Select<KeyValuePair<HealthStatus, int>, string>((Func<KeyValuePair<HealthStatus, int>, string>)(y => y.Key.ToString())))))
                {
                    var openApiResponse = new OpenApiResponse()
                    {
                        Description = string.Join(", ", keyValuePair.Value)
                    };
                    openApiResponse.Content.Add("text/plain", new OpenApiMediaType()
                    {
                        Schema = new OpenApiSchema()
                        {
                            Type = "string",
                            Description = openApiResponse.Description,
                            Example = (IOpenApiAny)new OpenApiString(keyValuePair.Value.First<string>())
                        },
                        Examples = (IDictionary<string, OpenApiExample>)keyValuePair.Value.ToDictionary<string, string, OpenApiExample>((Func<string, string>)(x => x), (Func<string, OpenApiExample>)(x => new OpenApiExample()
                        {
                            Value = (IOpenApiAny)new OpenApiString(x)
                        }))
                    });
                    operation.Responses.Add(keyValuePair.Key, openApiResponse);
                }
            }
            openApiPathItem.AddOperation(OperationType.Get, operation);
            swaggerDoc.Paths.Add(this._routePath, openApiPathItem);
        }
    }
}

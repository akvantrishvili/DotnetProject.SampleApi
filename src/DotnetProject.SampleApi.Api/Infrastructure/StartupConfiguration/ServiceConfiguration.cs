using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Asp.Versioning;
using Asp.Versioning.Conventions;
using DotnetProject.SampleApi.Api.Infrastructure.ErrorHandling;
using DotnetProject.SampleApi.Api.Infrastructure.OperationFilter;
using DotnetProject.SampleApi.Application;
using DotnetProject.SampleApi.Infrastructure;
using DotnetProject.SampleApi.Persistence;
using DotnetProject.SampleApi.Persistence.Database;
using FluentValidation;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DotnetProject.SampleApi.Api.Infrastructure.StartupConfiguration
{
    public static class ServiceConfiguration
    {
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddPersistence(builder.Configuration.GetConnectionString("Database") ?? throw new InvalidOperationException("Database connection string not configured"));
            builder.Services.AddInfrastructure();
            builder.Services.AddApplication(typeof(Program).Assembly);


            #region API and Validations

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(new ProducesAttribute("application/json"));
                options.Filters.Add(new ConsumesAttribute("application/json"));
            }).AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            builder.Services.AddProblemDetails(options => options.CustomizeProblemDetails = (contex) =>
            {
                contex.ProblemDetails.Extensions.Add("nodeId", Environment.MachineName);
            });

            builder.Services.AddExceptionHandler<ExceptionToProblemDetailsHandler>();

            //Disable automatic data annotation validations, because we are using fluent validations and MediatR validation pipeline behaviour.
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
                options.SuppressMapClientErrors = true;
            });

            builder.Services.AddValidatorsFromAssemblyContaining<Domain.Customers.Customer>();
            builder.Services.AddValidatorsFromAssemblyContaining<Application.Exceptions.ApplicationException>();
            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // See details here: https://confluence.tbcbank.ge/display/SD/API+Error+Handling
            //builder.Services.AddApiProblemDetailsFactory();

            #endregion API and Validations

            #region API Versioning

            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });
            builder.Services.AddApiVersioning(
                    options =>
                    {
                        options.DefaultApiVersion = new ApiVersion(1.0);
                        options.AssumeDefaultVersionWhenUnspecified = true;
                        // reporting api versions will return the headers
                        // "api-supported-versions" and "api-deprecated-versions"
                        options.ReportApiVersions = true;
                        options.ApiVersionReader = ApiVersionReader.Combine(
                            new UrlSegmentApiVersionReader(),
                            new QueryStringApiVersionReader("api-version"),
                            new HeaderApiVersionReader("X-Version"),
                            new MediaTypeApiVersionReader("x-version"));
                    })
                .AddMvc(
                    options =>
                    {
                        // automatically applies an api version based on the name of
                        // the defining controller's namespace
                        options.Conventions.Add(new VersionByNamespaceConvention());
                    })
                .AddApiExplorer(setup =>
                {
                    setup.GroupNameFormat = "'v'VVV";
                    setup.SubstituteApiVersionInUrl = true;
                });


            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "DontnetProject.SampleApi.Api",
                        Version = "v1",
                        Description = "DontnetProject.SampleApi API.",
                        Contact = new OpenApiContact
                        {
                            Name = "DontnetProject.SampleApi Team",
                            Email = "akvantrishvili@gmail.com",
                        }
                    });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                options.TagActionsBy(static x =>
                {
                    var routeName = (x.ActionDescriptor.EndpointMetadata
                        .FirstOrDefault(y => y is RouteAttribute) as RouteAttribute)?.Name;

                    if (!string.IsNullOrEmpty(routeName))
                        return [routeName];

                    return [x.ActionDescriptor.RouteValues["controller"].RemoveVersionNumberSuffix()];
                });
                options.ExampleFilters();
                options.DocumentFilter<HealthCheckDocumentFilter>("/health");
                options.CustomOperationIds(x => null);
                //options.AddErrorCodeDescriptions();

            });
            builder.Services.AddFluentValidationRulesToSwagger();
            builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());

            #endregion API Versioning

            #region Health checks

            builder.Services.AddHealthChecks()
                .AddDbContextCheck<AppDbContext>("Database");

            #endregion Health checks

            #region Localization
            // See details here: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
            var supportedCultures = new List<CultureInfo> { new("en"), new("ka") };
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(supportedCultures[0]);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders = [ new AcceptLanguageHeaderRequestCultureProvider
                {
                    Options = options
                }];
            });
            #endregion Localization

            #region Cors

            // Add CORS policy settings.
            // See details here: https://docs.microsoft.com/en-us/aspnet/core/security/cors
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policyBuilder =>
                    {
                        var allowedOrigins = builder.Configuration.GetValue<string>("CorsAllowedOrigins");
                        if (!string.IsNullOrEmpty(allowedOrigins))
                        {
                            var origins = allowedOrigins.Split(";");
                            policyBuilder.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod();

                            if (!origins.Contains("*", StringComparer.InvariantCultureIgnoreCase))
                            {
                                policyBuilder.AllowCredentials();
                            }
                        }
                    });
            });

            #endregion Cors

            return builder;
        }
        private static string? RemoveVersionNumberSuffix(this string? self)
        {
            return self is null ? null : Regex.Replace(self, @"([vV]{1}[0-9]+)?$", string.Empty);
        }
    }
}

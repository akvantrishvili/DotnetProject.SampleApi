// Copyright (C) TBC Bank. All Rights Reserved.

using System;
using System.Threading;
using System.Threading.Tasks;
using DotnetProject.SampleApi.Application.Exceptions;
using DotnetProject.SampleApi.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DotnetProject.SampleApi.Api.Infrastructure.ErrorHandling
{
    public class ExceptionToProblemDetailsHandler : IExceptionHandler
    {
        private readonly ILogger<ExceptionToProblemDetailsHandler> _logger;

        public ExceptionToProblemDetailsHandler(ILogger<ExceptionToProblemDetailsHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(
                exception, "Exception occurred: {Message}", exception.Message);

            switch (exception)
            {
               
                case ObjectNotFoundException objectNotFoundException:
                    httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                    httpContext.Response.ContentType = "application/problem+json";
                    await httpContext.Response.WriteAsJsonAsync(new
                    {
                        title = objectNotFoundException.Message,
                        status = StatusCodes.Status404NotFound,
                        detail = objectNotFoundException.Message,
                        code = objectNotFoundException.Code,
                        objectType = objectNotFoundException.ObjectType,
                        objectId = objectNotFoundException.ObjectId
                    }, cancellationToken).ConfigureAwait(true);
                    break;
                case ObjectAlreadyExistsException objectAlreadyExistsException:
                    httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                    httpContext.Response.ContentType = "application/problem+json";
                    await httpContext.Response.WriteAsJsonAsync(new
                    {
                        title = objectAlreadyExistsException.Message,
                        status = StatusCodes.Status409Conflict,
                        detail = objectAlreadyExistsException.Message,
                        code = objectAlreadyExistsException.Code,
                        objectType = objectAlreadyExistsException.ObjectType,
                        objectId = objectAlreadyExistsException.ObjectId
                    }, cancellationToken).ConfigureAwait(true);
                    break;
                case Application.Exceptions.ApplicationException applicationException:
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    httpContext.Response.ContentType = "application/problem+json";
                    await httpContext.Response.WriteAsJsonAsync(new
                    {
                        title = applicationException.Message,
                        status = StatusCodes.Status400BadRequest,
                        detail = applicationException.Message,
                        code = applicationException.Code
                    }, cancellationToken).ConfigureAwait(false);
                    break;
                case DomainException domainException:
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    httpContext.Response.ContentType = "application/problem+json";
                    await httpContext.Response.WriteAsJsonAsync(new
                    {
                        title = domainException.Message,
                        status = StatusCodes.Status400BadRequest,
                        detail = domainException.Message,
                        code = domainException.Code
                    }, cancellationToken).ConfigureAwait(true);
                    break;
                case FluentValidation.ValidationException:
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    httpContext.Response.ContentType = "application/problem+json";
                    await httpContext.Response.WriteAsJsonAsync(new
                    {
                        title = "Validation error occurred",
                        status = StatusCodes.Status400BadRequest,
                        detail = "Validation error occurred",
                        code = "ValidationError"
                    }, cancellationToken).ConfigureAwait(true);
                    break;
                default:
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    httpContext.Response.ContentType = "application/problem+json";
                    await httpContext.Response.WriteAsJsonAsync(new
                    {
                        title = "An error occurred while processing your request",
                        status = StatusCodes.Status500InternalServerError,
                        detail = "An error occurred while processing your request",
                        code = "InternalError"
                    }, cancellationToken).ConfigureAwait(true);
                    break;
            }
            return true;
        }
    }
}

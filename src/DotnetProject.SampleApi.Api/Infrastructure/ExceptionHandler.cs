

using System.Linq;
using DotnetProject.SampleApi.Application.Exceptions;
using DotnetProject.SampleApi.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ApplicationException = DotnetProject.SampleApi.Application.Exceptions.ApplicationException;

namespace DotnetProject.SampleApi.Api.Infrastructure
{
    // See details here: https://confluence.tbcbank.ge/display/SD/API+Error+Handling
    //public class ExceptionHandler :
    //    IExceptionHandler<ApplicationException>,
    //    IExceptionHandler<ObjectNotFoundException>,
    //    IExceptionHandler<ObjectAlreadyExistsException>,
    //    IExceptionHandler<CommandValidationException>,
    //    IExceptionHandler<FluentValidation.ValidationException>,
    //    IExceptionHandler<DomainException>
    //{
    //    public void Handle(ApiProblemContext<ApiProblemDetails, ApplicationException> ctx)
    //    {
    //        ctx.ProblemDetails.Status = StatusCodes.Status500InternalServerError;
    //        ctx.ProblemDetails.Code = ctx.Exception.Code;
    //        ctx.ProblemDetails.Title = ctx.Exception.Message;
    //        ctx.ProblemDetails.Detail = ctx.Exception.Message;
    //        ctx.Logging.LogLevel = LogLevel.Warning;
    //    }

    //    public void Handle(ApiProblemContext<ApiProblemDetails, ObjectNotFoundException> ctx)
    //    {
    //        ctx.ProblemDetails.Status = StatusCodes.Status404NotFound;
    //        ctx.ProblemDetails.Code = ctx.Exception.Code;
    //        ctx.ProblemDetails.Title = ctx.Exception.Message;
    //        ctx.ProblemDetails.Detail = ctx.Exception.Message;
    //        ctx.ProblemDetails.Extensions["objectType"] = ctx.Exception.ObjectType;
    //        ctx.ProblemDetails.Extensions["objectId"] = ctx.Exception.ObjectId.ToString();
    //        ctx.Logging.LogLevel = LogLevel.Information;
    //    }

    //    public void Handle(ApiProblemContext<ApiProblemDetails, ObjectAlreadyExistsException> ctx)
    //    {
    //        ctx.ProblemDetails.Status = StatusCodes.Status409Conflict;
    //        ctx.ProblemDetails.Code = ctx.Exception.Code;
    //        ctx.ProblemDetails.Title = ctx.Exception.Message;
    //        ctx.ProblemDetails.Detail = ctx.Exception.Message;
    //        ctx.ProblemDetails.Extensions["objectType"] = ctx.Exception.ObjectType;
    //        ctx.ProblemDetails.Extensions["objectId"] = ctx.Exception.ObjectId.ToString();
    //        ctx.Logging.LogLevel = LogLevel.Information;
    //    }

    //    public void Handle(ApiProblemContext<ApiProblemDetails, CommandValidationException> ctx)
    //    {
    //        ctx.ProblemDetails.Status = StatusCodes.Status400BadRequest;
    //        ctx.ProblemDetails.Code = DefaultErrorCodes.ValidationError.Code;
    //        ctx.ProblemDetails.Title = DefaultErrorCodes.ValidationError.Title;
    //        ctx.ProblemDetails.Errors = ctx.Exception.Errors.GroupBy(x => x.PropertyName ?? string.Empty)
    //            .ToDictionary(x => x.Key,
    //                y => y.Select(z => z.ErrorMessage).ToArray())!;
    //        ctx.Logging.LogLevel = LogLevel.Information;
    //    }

    //    public void Handle(ApiProblemContext<ApiProblemDetails, FluentValidation.ValidationException> ctx)
    //    {
    //        ctx.ProblemDetails.Status = StatusCodes.Status400BadRequest;
    //        ctx.ProblemDetails.Code = DefaultErrorCodes.ValidationError.Code;
    //        ctx.ProblemDetails.Title = DefaultErrorCodes.ValidationError.Title;
    //        ctx.ProblemDetails.Errors = ctx.Exception.Errors.GroupBy(x => x.PropertyName ?? string.Empty)
    //            .ToDictionary(x => x.Key,
    //                y => y.Select(z => z.ErrorMessage).ToArray())!;
    //        ctx.Logging.LogLevel = LogLevel.Information;
    //    }

    //    public void Handle(ApiProblemContext<ApiProblemDetails, DomainException> ctx)
    //    {
    //        ctx.ProblemDetails.Status = StatusCodes.Status400BadRequest;
    //        ctx.ProblemDetails.Code = ctx.Exception.Code;
    //        ctx.ProblemDetails.Title = ctx.Exception.Message;
    //        ctx.ProblemDetails.Detail = ctx.Exception.Message;
    //        ctx.Logging.LogLevel = LogLevel.Information;
    //    }
    //}
}

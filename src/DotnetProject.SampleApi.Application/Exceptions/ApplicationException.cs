

using System;

namespace DotnetProject.SampleApi.Application.Exceptions
{
    public class ApplicationException(
        string? code,
        string? title,
        string? message = null,
        Exception? innerException = null)
        : Exception(message ?? title ?? code, innerException)
    {
        public string Code { get; } = code ?? ErrorCodeDescription;
        public string Title { get; } = title ?? ErrorCodeDescription;
        private const string ErrorCodeDescription = "Application logic validation error";

        public ApplicationException() : this("ApplicationError",
            ErrorCodeDescription)
        { }

        public ApplicationException(string title) : this("ApplicationError",
            title)
        { }
    }
}


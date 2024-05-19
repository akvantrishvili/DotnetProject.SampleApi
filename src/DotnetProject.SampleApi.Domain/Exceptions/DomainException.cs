

using System;

namespace DotnetProject.SampleApi.Domain.Exceptions
{
    public class DomainException(string? code, string? title, string? message = null, Exception? innerException = null) : Exception
    {
        public string Code { get; private set; } = code ?? "DomainError";
        public string Title { get; private set; } = title ?? ErrorTitle;
        private const string ErrorTitle = "Domain logic validation error";

        public DomainException() : this("DomainError",
            ErrorTitle)
        { }

        public DomainException(string title) : this("DomainError",
            title)
        { }
    }

}

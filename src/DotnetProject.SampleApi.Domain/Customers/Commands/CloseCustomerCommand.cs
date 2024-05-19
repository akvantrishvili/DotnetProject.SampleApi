

using System.Text.Json.Serialization;
using FluentValidation;
using MediatR;

namespace DotnetProject.SampleApi.Domain.Customers.Commands
{
    /// <summary>
    /// Close customer
    /// </summary>
    public sealed class CloseCustomerCommand : IRequest
    {
        [JsonIgnore]
        public long CustomerId { get; set; }
    }

    public sealed class CloseCustomerCommandValidator : AbstractValidator<CloseCustomerCommand>
    {
        public CloseCustomerCommandValidator()
        {
            RuleFor(x => x.CustomerId).NotNull()
                .GreaterThan(0);
        }
    }
}

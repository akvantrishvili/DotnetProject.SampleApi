

using System.Text.Json.Serialization;
using FluentValidation;
using MediatR;

namespace DotnetProject.SampleApi.Domain.Customers.Commands
{
    /// <summary>
    /// Change customer address
    /// </summary>
    public sealed class ChangeAddressCommand : IRequest
    {
        [JsonIgnore]
        public long CustomerId { get; set; }

        public CreateAddressCommand ActualAddress { get; set; } = null!;
        public CreateAddressCommand LegalAddress { get; set; } = null!;
    }

    public sealed class ChangeAddressCommandValidator : AbstractValidator<ChangeAddressCommand>
    {
        public ChangeAddressCommandValidator()
        {
            RuleFor(x => x.CustomerId).NotNull()
                .GreaterThan(0);

            RuleFor(x => x.ActualAddress)
                .NotNull().SetValidator(new CreateAddressCommandValidator());

            RuleFor(x => x.LegalAddress)
                .NotNull().SetValidator(new CreateAddressCommandValidator());
        }
    }
}



using FluentValidation;

namespace DotnetProject.SampleApi.Domain.Customers.Commands
{
    public sealed class CreateAddressCommand
    {
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? ZipCode { get; set; }
        public string Address1 { get; set; } = null!;
        public string? Address2 { get; set; }
    }

    public sealed class CreateAddressCommandValidator : AbstractValidator<CreateAddressCommand>
    {
        public CreateAddressCommandValidator()
        {
            RuleFor(x => x.Country)
                .NotEmpty().Length(2, 50);

            RuleFor(x => x.City)
                .NotEmpty().Length(2, 50);

            RuleFor(x => x.ZipCode)
                .MaximumLength(50);

            RuleFor(x => x.Address1)
                .NotEmpty().Length(2, 250);

            RuleFor(x => x.Address2)
                .MaximumLength(250);
        }
    }
}



using System;
using System.Text.Json.Serialization;
using FluentValidation;
using MediatR;

namespace DotnetProject.SampleApi.Domain.Customers.Commands
{
    /// <summary>
    /// Change customer basic info, e.g. name, date of birth, ...
    /// </summary>
    public sealed class ChangeBasicInfoCommand : IRequest
    {
        [JsonIgnore]
        public long CustomerId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
    }

    public sealed class ChangeBasicInfoCommandValidator : AbstractValidator<ChangeBasicInfoCommand>
    {
        public ChangeBasicInfoCommandValidator()
        {
            RuleFor(x => x.CustomerId).NotNull()
                .GreaterThan(0);

            RuleFor(x => x.FirstName)
                .NotEmpty().Length(Customer.NameMinLength, Customer.NameMaxLength);

            RuleFor(x => x.LastName)
                .NotEmpty().Length(Customer.NameMinLength, Customer.NameMaxLength);

            RuleFor(x => x.DateOfBirth).NotNull()
                .GreaterThan(x => DateTime.Now.AddYears(-Customer.MaxAge));

            RuleFor(x => x.Gender).NotNull()
                .IsInEnum();
        }
    }
}

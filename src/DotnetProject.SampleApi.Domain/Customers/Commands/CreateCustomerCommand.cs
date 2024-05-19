

using System;
using System.Collections.Generic;
using DotnetProject.SampleApi.Domain.Customers.Commands.IdentityDocuments;
using FluentValidation;
using MediatR;

namespace DotnetProject.SampleApi.Domain.Customers.Commands
{
    /// <summary>
    /// Create customer command
    /// </summary>
    public sealed class CreateCustomerCommand : IRequest<long>
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public CreateAddressCommand ActualAddress { get; set; } = null!;
        public CreateAddressCommand LegalAddress { get; set; } = null!;
        public List<CreateIdentityDocumentCommand> IdentityDocuments { get; set; } = null!;
    }

    public sealed class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().Length(Customer.NameMinLength, Customer.NameMaxLength);

            RuleFor(x => x.LastName)
                .NotEmpty().Length(Customer.NameMinLength, Customer.NameMaxLength);

            RuleFor(x => x.DateOfBirth).NotNull()
                .GreaterThan(x => DateTime.Now.AddYears(-Customer.MaxAge));

            RuleFor(x => x.Gender).NotNull()
                .IsInEnum();

            RuleFor(x => x.ActualAddress)
                .NotNull().SetValidator(new CreateAddressCommandValidator());

            RuleFor(x => x.LegalAddress)
                .NotNull().SetValidator(new CreateAddressCommandValidator());

            RuleFor(x => x.IdentityDocuments)
                .NotEmpty();
            RuleForEach(x => x.IdentityDocuments)
                .SetValidator(new CreateIdentityDocumentCommandValidator());
        }
    }
}

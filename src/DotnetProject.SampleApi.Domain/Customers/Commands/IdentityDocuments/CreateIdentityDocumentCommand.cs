

using System;
using FluentValidation;

namespace DotnetProject.SampleApi.Domain.Customers.Commands.IdentityDocuments
{
    public class CreateIdentityDocumentCommand
    {
        public IdentityDocumentType DocumentType { get; set; }
        public string DocumentId { get; set; } = null!;
        public string PersonalId { get; set; } = null!;
        public DateTime DateOfIssue { get; set; }
        public DateTime? DateOfExpire { get; set; }
    }

    public sealed class CreateIdentityDocumentCommandValidator : AbstractValidator<CreateIdentityDocumentCommand>
    {
        public CreateIdentityDocumentCommandValidator()
        {
            RuleFor(x => x.DocumentType).NotNull().IsInEnum();

            RuleFor(x => x.DocumentId)
                .NotEmpty().Length(2, 50);

            RuleFor(x => x.PersonalId)
                .NotEmpty().Length(2, 50);

            RuleFor(x => x.DateOfIssue).NotNull()
                .GreaterThan(DateTime.Now.AddYears(-150));

            RuleFor(x => x.DateOfExpire)
                .Must((command, field) => !field.HasValue || field.Value > command.DateOfIssue);
        }
    }
}

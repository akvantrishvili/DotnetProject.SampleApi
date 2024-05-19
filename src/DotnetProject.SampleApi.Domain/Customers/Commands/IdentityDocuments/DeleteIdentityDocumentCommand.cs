

using FluentValidation;
using MediatR;

namespace DotnetProject.SampleApi.Domain.Customers.Commands.IdentityDocuments
{
    /// <summary>
    /// Delete identity document
    /// </summary>
    public sealed class DeleteIdentityDocumentCommand : IRequest
    {
        public long CustomerId { get; set; }
        public long IdentityDocumentId { get; set; }
    }

    public sealed class DeleteIdentityDocumentCommandValidator : AbstractValidator<DeleteIdentityDocumentCommand>
    {
        public DeleteIdentityDocumentCommandValidator()
        {
            RuleFor(x => x.CustomerId).NotNull()
                .GreaterThan(0);

            RuleFor(x => x.IdentityDocumentId).NotNull()
                .GreaterThan(0);
        }
    }
}

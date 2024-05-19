

using System.Text.Json.Serialization;
using FluentValidation;
using MediatR;

namespace DotnetProject.SampleApi.Domain.Customers.Commands.IdentityDocuments
{
    /// <summary>
    /// Add identity document to customer
    /// </summary>
    public sealed class AddIdentityDocumentCommand : CreateIdentityDocumentCommand, IRequest
    {
        [JsonIgnore]
        public long CustomerId { get; set; }
    }

    public sealed class AddIdentityDocumentCommandValidator : AbstractValidator<AddIdentityDocumentCommand>
    {
        public AddIdentityDocumentCommandValidator()
        {
            Include(new CreateIdentityDocumentCommandValidator());
            RuleFor(x => x.CustomerId).NotNull()
                .GreaterThan(0);
        }
    }
}

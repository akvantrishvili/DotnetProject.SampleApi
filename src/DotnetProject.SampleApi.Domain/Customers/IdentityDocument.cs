

using System;
using System.ComponentModel.DataAnnotations;
using DotnetProject.SampleApi.Domain.Common;
using DotnetProject.SampleApi.Domain.Customers.Commands.IdentityDocuments;

namespace DotnetProject.SampleApi.Domain.Customers
{
    public class IdentityDocument : Entity<long>
    {
        public IdentityDocumentType DocumentType { get; private set; }

        [Required, MaxLength(50), MinLength(2)]
        public string DocumentId { get; private set; } = null!;

        [Required, MaxLength(50), MinLength(2)]
        public string PersonalId { get; private set; } = null!;

        [Required]
        public DateTime DateOfIssue { get; private set; }

        public DateTime? DateOfExpire { get; private set; }

        #region Reference properties

        public long CustomerId { get; private set; }

        public Customer? Customer { get; private set; }

        #endregion Reference properties

        private IdentityDocument() { }

        public IdentityDocument(CreateIdentityDocumentCommand command)
        {
            DocumentType = command.DocumentType;
            DocumentId = command.DocumentId;
            PersonalId = command.PersonalId;
            DateOfIssue = command.DateOfIssue;
            DateOfExpire = command.DateOfExpire;
        }
    }
}

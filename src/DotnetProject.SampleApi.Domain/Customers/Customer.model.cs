

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DotnetProject.SampleApi.Domain.Customers
{
    /// <summary>
    /// Customer
    /// </summary>
    public partial class Customer
    {
        #region Constants

        public const int NameMinLength = 2;
        public const int NameMaxLength = 50;
        public const int MaxAge = 150;

        #endregion Constants

        [MaxLength(NameMaxLength), MinLength(NameMinLength)]
        public string FirstName { get; private set; } = null!;

        [MaxLength(NameMaxLength), MinLength(NameMinLength)]
        public string LastName { get; private set; } = null!;

        public DateTime DateOfBirth { get; private set; }

        public Gender Gender { get; private set; }

        public CustomerStatus Status { get; private set; }

        public DateTime OpenDate { get; private set; }

        public DateTime? CloseDate { get; private set; }

        public List<IdentityDocument>? IdentityDocuments { get; private set; }

        public CustomerAddress ActualAddress { get; private set; } = null!;

        public CustomerAddress LegalAddress { get; private set; } = null!;

    }
}

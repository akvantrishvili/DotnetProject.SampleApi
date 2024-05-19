

using System;
using System.Collections.Generic;
using System.Linq;
using DotnetProject.SampleApi.Domain.Common;
using DotnetProject.SampleApi.Domain.Customers.Commands;
using DotnetProject.SampleApi.Domain.Customers.Commands.IdentityDocuments;
using DotnetProject.SampleApi.Domain.Exceptions;

namespace DotnetProject.SampleApi.Domain.Customers
{
    public partial class Customer : AggregateRoot<long>
    {
        private Customer() { }

        public Customer(CreateCustomerCommand command)
        {
            FirstName = command.FirstName;
            LastName = command.LastName;
            DateOfBirth = command.DateOfBirth;
            Gender = command.Gender;
            Status = CustomerStatus.Open;
            OpenDate = DateTime.Now;
            IdentityDocuments = command.IdentityDocuments.Select(x => new IdentityDocument(x)).ToList();
            ActualAddress = new CustomerAddress(command.ActualAddress);
            LegalAddress = new CustomerAddress(command.LegalAddress);
        }

        public void ChangeBasicInfo(ChangeBasicInfoCommand command)
        {
            CheckCustomerStatus();

            FirstName = command.FirstName;
            LastName = command.LastName;
            DateOfBirth = command.DateOfBirth;
            Gender = command.Gender;
        }

        public void AddIdentityDocument(AddIdentityDocumentCommand command)
        {
            CheckCustomerStatus();

            IdentityDocuments ??= [];

            if (IdentityDocuments.Exists(x => x.DocumentType == command.DocumentType && x.PersonalId == command.PersonalId))
                return;

            var sameTypeOfDocument = IdentityDocuments.Find(x => x.DocumentType == command.DocumentType
                                                                 && command.DateOfIssue >= x.DateOfIssue
                                                                 && x.DateOfExpire != null
                                                                 && command.DateOfIssue < x.DateOfExpire);
            if (sameTypeOfDocument != null)
                throw new DomainException("Identity Document with the same type and period already exists");

            IdentityDocuments.Add(new IdentityDocument(command));
        }

        public void DeleteIdentityDocument(DeleteIdentityDocumentCommand command)
        {
            CheckCustomerStatus();

            IdentityDocuments ??= [];

            var doc = IdentityDocuments.Find(x => x.Id == command.IdentityDocumentId);
            if (doc == null)
                return;

            IdentityDocuments.Remove(doc);
        }

        public void ChangeAddress(ChangeAddressCommand command)
        {
            CheckCustomerStatus();

            LegalAddress = new CustomerAddress(command.LegalAddress);
            ActualAddress = new CustomerAddress(command.ActualAddress);
        }

        public void CloseCustomer(CloseCustomerCommand command)
        {
            if (Status == CustomerStatus.Closed)
                return;

            Status = CustomerStatus.Closed;
            CloseDate = DateTime.Now;
        }

        private void CheckCustomerStatus()
        {
            if (Status == CustomerStatus.Closed)
                throw new DomainException("Closed customer changes are not allowed");
        }
    }
}

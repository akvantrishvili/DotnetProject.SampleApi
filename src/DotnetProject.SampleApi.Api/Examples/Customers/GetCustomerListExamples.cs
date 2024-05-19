

using System;
using System.Collections.Generic;
using DotnetProject.SampleApi.Application.Common;
using DotnetProject.SampleApi.Domain.Customers;
using DotnetProject.SampleApi.Domain.Customers.Commands;
using DotnetProject.SampleApi.Domain.Customers.Commands.IdentityDocuments;
using Swashbuckle.AspNetCore.Filters;

namespace DotnetProject.SampleApi.Api.Examples.Customers
{
    public class GetCustomerListExamples : IExamplesProvider<PagedList<Customer>>
    {
        public PagedList<Customer> GetExamples()
        {
            return new PagedList<Customer>(
            [
                new(new CreateCustomerCommand
                {
                    FirstName = "Giorgi",
                    LastName = "Giorgadze",
                    DateOfBirth = new DateTime(1980, 05, 06),
                    Gender = Gender.Male,
                    IdentityDocuments =
                    [
                        new CreateIdentityDocumentCommand
                        {
                            DocumentType = IdentityDocumentType.IdCard,
                            PersonalId = "01010101010",
                            DocumentId = "AB010203",
                            DateOfIssue = new DateTime(2009, 05, 06),
                            DateOfExpire = new DateTime(2029, 05, 06)
                        }
                    ],
                    ActualAddress = new CreateAddressCommand
                    {
                        Country = "Georgia",
                        City = "Tbilisi",
                        ZipCode = "0100",
                        Address1 = "Chavchavadze str. 5"
                    },
                    LegalAddress = new CreateAddressCommand
                    {
                        Country = "Georgia",
                        City = "Tbilisi",
                        ZipCode = "0100",
                        Address1 = "Melikishvili str. 10"
                    }
                }),
                new(new CreateCustomerCommand
                {
                    FirstName = "Nino",
                    LastName = "Ninoshvili",
                    DateOfBirth = new DateTime(1991, 07, 12),
                    Gender = Gender.Female,
                    IdentityDocuments =
                    [
                        new() {
                            DocumentType = IdentityDocumentType.IdCard,
                            PersonalId = "02020101010",
                            DocumentId = "AC020203",
                            DateOfIssue = new DateTime(2015, 10, 20),
                            DateOfExpire = new DateTime(2025, 10, 20)
                        }
                    ],
                    ActualAddress = new CreateAddressCommand
                    {
                        Country = "Georgia",
                        City = "Kutaisi",
                        ZipCode = "0200",
                        Address1 = "Rustaveli str. 5"
                    },
                    LegalAddress = new CreateAddressCommand
                    {
                        Country = "Georgia",
                        City = "Kutaisi",
                        ZipCode = "0200",
                        Address1 = "Rustaveli str. 5"
                    }
                })
            ], 0, 10, 2);
        }
    }
}

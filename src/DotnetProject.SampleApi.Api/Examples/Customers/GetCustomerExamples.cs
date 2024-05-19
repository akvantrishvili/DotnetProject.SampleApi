

using System;
using System.Collections.Generic;
using DotnetProject.SampleApi.Domain.Customers;
using DotnetProject.SampleApi.Domain.Customers.Commands;
using DotnetProject.SampleApi.Domain.Customers.Commands.IdentityDocuments;
using Swashbuckle.AspNetCore.Filters;

namespace DotnetProject.SampleApi.Api.Examples.Customers
{
    public class GetCustomerExamples : IMultipleExamplesProvider<Customer>
    {
        public IEnumerable<SwaggerExample<Customer>> GetExamples()
        {
            yield return SwaggerExample.Create("Giorgi", new Customer(new CreateCustomerCommand
            {
                FirstName = "Giorgi",
                LastName = "Giorgadze",
                DateOfBirth = new DateTime(1980, 05, 06, 0, 0, 0, DateTimeKind.Local),
                Gender = Gender.Male,
                IdentityDocuments = new List<CreateIdentityDocumentCommand>(1)
                {
                    new() {
                        DocumentType = IdentityDocumentType.IdCard,
                        PersonalId = "01010101010",
                        DocumentId = "AB010203",
                        DateOfIssue = new DateTime(2009, 05, 06, 0, 0, 0, DateTimeKind.Local),
                        DateOfExpire = new DateTime(2029, 05, 06, 0, 0, 0, DateTimeKind.Local)
                    }
                },
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
            }));

            yield return SwaggerExample.Create("Nino", new Customer(new CreateCustomerCommand
            {
                FirstName = "Nino",
                LastName = "Ninoshvili",
                DateOfBirth = new DateTime(1991, 07, 12, 0, 0, 0, DateTimeKind.Local),
                Gender = Gender.Female,
                IdentityDocuments = new List<CreateIdentityDocumentCommand>(1)
                {
                    new() {
                        DocumentType = IdentityDocumentType.IdCard,
                        PersonalId = "02020101010",
                        DocumentId = "AC020203",
                        DateOfIssue = new DateTime(2015, 10, 20, 0, 0, 0, DateTimeKind.Local),
                        DateOfExpire = new DateTime(2025, 10, 20, 0, 0, 0, DateTimeKind.Local)
                    }
                },
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
            }));
        }
    }
}



using System;
using System.Collections.Generic;
using DotnetProject.SampleApi.Domain.Customers;
using DotnetProject.SampleApi.Domain.Customers.Commands.IdentityDocuments;
using Swashbuckle.AspNetCore.Filters;

namespace DotnetProject.SampleApi.Api.Examples.Customers
{
    public class AddIdentityDocumentCommandExamples : IMultipleExamplesProvider<AddIdentityDocumentCommand>
    {
        public IEnumerable<SwaggerExample<AddIdentityDocumentCommand>> GetExamples()
        {
            yield return SwaggerExample.Create("Passport", new AddIdentityDocumentCommand
            {
                CustomerId = 1,
                DocumentType = IdentityDocumentType.Passport,
                PersonalId = "01010101010",
                DocumentId = "AB010101",
                DateOfIssue = new DateTime(2010, 05, 07, 0, 0, 0, DateTimeKind.Local),
                DateOfExpire = new DateTime(2025, 05, 07, 0, 0, 0, DateTimeKind.Local)
            });

            yield return SwaggerExample.Create("Driver License", new AddIdentityDocumentCommand
            {
                CustomerId = 1,
                DocumentType = IdentityDocumentType.DriverLicense,
                PersonalId = "02010101",
                DocumentId = "AC010101",
                DateOfIssue = new DateTime(2015, 10, 02, 0, 0, 0, DateTimeKind.Local),
                DateOfExpire = new DateTime(2030, 10, 02, 0, 0, 0, DateTimeKind.Local)
            });
        }
    }
}

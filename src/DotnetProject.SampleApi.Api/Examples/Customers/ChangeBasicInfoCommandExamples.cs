

using System;
using System.Collections.Generic;
using DotnetProject.SampleApi.Domain.Customers;
using DotnetProject.SampleApi.Domain.Customers.Commands;
using Swashbuckle.AspNetCore.Filters;

namespace DotnetProject.SampleApi.Api.Examples.Customers
{
    public class ChangeBasicInfoCommandExamples : IMultipleExamplesProvider<ChangeBasicInfoCommand>
    {
        public IEnumerable<SwaggerExample<ChangeBasicInfoCommand>> GetExamples()
        {
            yield return SwaggerExample.Create("Giorgi", new ChangeBasicInfoCommand
            {
                CustomerId = 1,
                FirstName = "Giorgi",
                LastName = "Giorgadze",
                DateOfBirth = new DateTime(1980, 05, 06, 0, 0, 0, DateTimeKind.Local),
                Gender = Gender.Male
            });

            yield return SwaggerExample.Create("Nino", new ChangeBasicInfoCommand
            {
                CustomerId = 2,
                FirstName = "Nino",
                LastName = "Ninoshvili",
                DateOfBirth = new DateTime(1991, 07, 12, 0, 0, 0, DateTimeKind.Local),
                Gender = Gender.Female
            });
        }
    }
}



using System.Collections.Generic;
using DotnetProject.SampleApi.Domain.Customers.Commands;
using Swashbuckle.AspNetCore.Filters;

namespace DotnetProject.SampleApi.Api.Examples.Customers
{
    public class ChangeAddressCommandExamples : IMultipleExamplesProvider<ChangeAddressCommand>
    {
        public IEnumerable<SwaggerExample<ChangeAddressCommand>> GetExamples()
        {
            yield return SwaggerExample.Create("Giorgi", new ChangeAddressCommand
            {
                CustomerId = 1,
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
            });

            yield return SwaggerExample.Create("Nino", new ChangeAddressCommand
            {
                CustomerId = 2,
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
            });
        }
    }
}

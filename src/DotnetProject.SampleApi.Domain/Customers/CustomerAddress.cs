

using System.ComponentModel.DataAnnotations;
using DotnetProject.SampleApi.Domain.Customers.Commands;

namespace DotnetProject.SampleApi.Domain.Customers
{
    public record CustomerAddress
    {
        [Required, MaxLength(50), MinLength(2)]
        public string Country { get; init; } = null!;

        [Required, MaxLength(50), MinLength(2)]
        public string City { get; init; } = null!;

        [MaxLength(50)]
        public string? ZipCode { get; init; }

        [Required, MaxLength(250), MinLength(2)]
        public string Address1 { get; init; } = null!;

        [MaxLength(250)]
        public string? Address2 { get; init; }

        private CustomerAddress() { }

        public CustomerAddress(CreateAddressCommand command)
        {
            Country = command.Country;
            City = command.City;
            ZipCode = command.ZipCode;
            Address1 = command.Address1;
            Address2 = command.Address2;
        }
    }
}

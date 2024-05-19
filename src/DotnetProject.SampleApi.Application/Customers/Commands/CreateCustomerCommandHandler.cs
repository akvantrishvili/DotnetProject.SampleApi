

using System.Threading;
using System.Threading.Tasks;
using DotnetProject.SampleApi.Application.Contracts;
using DotnetProject.SampleApi.Domain.Customers;
using DotnetProject.SampleApi.Domain.Customers.Commands;
using MediatR;

namespace DotnetProject.SampleApi.Application.Customers.Commands
{
    internal sealed class CreateCustomerCommandHandler(ICustomerRepository repository)
        : IRequestHandler<CreateCustomerCommand, long>
    {
        public async Task<long> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            var customer = new Customer(command);
            await repository.InsertAsync(customer, cancellationToken).ConfigureAwait(false);
            return customer.Id;
        }
    }
}

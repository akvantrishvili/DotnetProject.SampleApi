

using System.Threading;
using System.Threading.Tasks;
using DotnetProject.SampleApi.Application.Contracts;
using DotnetProject.SampleApi.Application.Exceptions;
using DotnetProject.SampleApi.Domain.Customers;
using DotnetProject.SampleApi.Domain.Customers.Commands;
using MediatR;

namespace DotnetProject.SampleApi.Application.Customers.Commands
{
    internal sealed class ChangeAddressCommandHandler(ICustomerRepository repository)
        : IRequestHandler<ChangeAddressCommand>
    {
        public async Task Handle(ChangeAddressCommand command, CancellationToken cancellationToken)
        {
            var customer = await repository.GetForUpdateAsync(command.CustomerId, [nameof(Customer.IdentityDocuments)], cancellationToken).ConfigureAwait(false) ?? throw new ObjectNotFoundException<Customer>(command.CustomerId);
            customer.ChangeAddress(command);

            await repository.UpdateAsync(customer, cancellationToken).ConfigureAwait(false);
        }
    }
}

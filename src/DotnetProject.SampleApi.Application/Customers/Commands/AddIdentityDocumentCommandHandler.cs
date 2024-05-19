

using System.Threading;
using System.Threading.Tasks;
using DotnetProject.SampleApi.Application.Contracts;
using DotnetProject.SampleApi.Application.Exceptions;
using DotnetProject.SampleApi.Domain.Customers;
using DotnetProject.SampleApi.Domain.Customers.Commands.IdentityDocuments;
using MediatR;

namespace DotnetProject.SampleApi.Application.Customers.Commands
{
    internal sealed class AddIdentityDocumentCommandHandler(ICustomerRepository repository)
        : IRequestHandler<AddIdentityDocumentCommand>
    {
        public async Task Handle(AddIdentityDocumentCommand command, CancellationToken cancellationToken)
        {
            var customer = await repository.GetForUpdateAsync(command.CustomerId, [nameof(Customer.IdentityDocuments)], cancellationToken)
                .ConfigureAwait(false) ?? throw new ObjectNotFoundException<Customer>(command.CustomerId);
            customer.AddIdentityDocument(command);
            await repository.UpdateAsync(customer, cancellationToken).ConfigureAwait(false);
        }
    }
}

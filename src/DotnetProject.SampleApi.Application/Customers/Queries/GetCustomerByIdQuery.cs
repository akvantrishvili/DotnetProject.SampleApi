using System.Threading;
using System.Threading.Tasks;
using DotnetProject.SampleApi.Application.Contracts;
using DotnetProject.SampleApi.Domain.Customers;
using MediatR;

namespace DotnetProject.SampleApi.Application.Customers.Queries
{
    public sealed class GetCustomerByIdQuery : IRequest<Customer?>
    {
        public long Id { get; set; }
    }

    internal sealed class GetCustomerByIdQueryHandler(ICustomerRepository repository)
        : IRequestHandler<GetCustomerByIdQuery, Customer?>
    {
        public Task<Customer?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            return repository.GetAsync(request.Id, [nameof(Customer.IdentityDocuments)], cancellationToken);
        }
    }
}

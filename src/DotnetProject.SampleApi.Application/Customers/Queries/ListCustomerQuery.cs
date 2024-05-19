

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotnetProject.SampleApi.Application.Common;
using DotnetProject.SampleApi.Application.Contracts;
using DotnetProject.SampleApi.Domain.Customers;
using MediatR;

namespace DotnetProject.SampleApi.Application.Customers.Queries
{
    public sealed class ListCustomerQuery : IRequest<PagedList<Customer>>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; } = 100;

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? IdNumber { get; set; }

        public CustomerStatus? Status { get; set; }
    }

    internal sealed class ListCustomerQueryHandler(ICustomerRepository repository)
        : IRequestHandler<ListCustomerQuery, PagedList<Customer>>
    {
        public Task<PagedList<Customer>> Handle(ListCustomerQuery request, CancellationToken cancellationToken)
        {
            var filter = PredicateBuilder.True<Customer>();
            if (!string.IsNullOrWhiteSpace(request.FirstName))
                filter = filter.And(x => x.FirstName.StartsWith(request.FirstName));
            if (!string.IsNullOrWhiteSpace(request.LastName))
                filter = filter.And(x => x.LastName.StartsWith(request.LastName));
            if (!string.IsNullOrWhiteSpace(request.IdNumber))
                filter = filter.And(x => x.IdentityDocuments!.Any(y => y.DocumentType == IdentityDocumentType.IdCard && y.PersonalId == request.IdNumber));
            if (request.Status.HasValue)
                filter = filter.And(x => x.Status == request.Status);

            return repository.GetPagedListAsync(
                request.PageIndex, request.PageSize,
                [nameof(Customer.IdentityDocuments)],
                filter,
                cancellationToken: cancellationToken);
        }
    }
}

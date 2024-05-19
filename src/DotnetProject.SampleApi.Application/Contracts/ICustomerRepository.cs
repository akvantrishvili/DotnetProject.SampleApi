

using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DotnetProject.SampleApi.Application.Common;
using DotnetProject.SampleApi.Domain.Customers;

namespace DotnetProject.SampleApi.Application.Contracts
{
    public interface ICustomerRepository
    {
        Task<PagedList<Customer>> GetPagedListAsync(
            int pageIndex, int pageSize,
            string[]? relatedProperties = null,
            Expression<Func<Customer, bool>>? predicate = null,
            SortingDetails? sortingDetails = null,
            CancellationToken cancellationToken = default);
        Task<Customer?> GetForUpdateAsync(
            long key,
            string[]? relatedProperties = null,
            CancellationToken cancellationToken = default);
        Task<Customer?> GetAsync(
            long key,
            string[]? relatedProperties = null,
            CancellationToken cancellationToken = default);
        Task<Customer> UpdateAsync(Customer entity, CancellationToken cancellationToken = default);
        Task<Customer> InsertAsync(Customer entity, CancellationToken cancellationToken = default);
    }
}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DotnetProject.SampleApi.Application.Common;
using DotnetProject.SampleApi.Application.Contracts;
using DotnetProject.SampleApi.Domain.Customers;
using DotnetProject.SampleApi.Persistence.Database;
using DotnetProject.SampleApi.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;


namespace DotnetProject.SampleApi.Persistence.Repositories
{
    public class CustomerRepository(AppDbContext context)
        : BaseRepository<AppDbContext, Customer>(context), ICustomerRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<Customer?> GetForUpdateAsync(long key, string[]? relatedProperties = null,
            CancellationToken cancellationToken = default)
        {
            var query = Table.ApplyIncludes(relatedProperties).AsQueryable().AsTracking();
            return await query.FirstOrDefaultAsync(t => t.Id == key, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Customer?> GetAsync(long key, string[]? relatedProperties = null,
            CancellationToken cancellationToken = default)
        {
            var query = Table.ApplyIncludes(relatedProperties).AsQueryable().AsNoTracking();
            return await query.FirstOrDefaultAsync(t => t.Id == key, cancellationToken).ConfigureAwait(false);
        }

        public async Task<PagedList<Customer>> GetPagedListAsync(int pageIndex, int pageSize,
            string[]? relatedProperties = null, Expression<Func<Customer, bool>>? predicate = null,
            SortingDetails? sortingDetails = null, CancellationToken cancellationToken = default)
        {
            var query = Table.ApplyIncludes(relatedProperties).AsNoTracking();

            if (predicate != null)
                query = query.Where(predicate);

            query = query.OrderBy(sortingDetails is { SortItems.Count: > 0 } ? sortingDetails : GetDefaultSorting());

            var count = await query.CountAsync(cancellationToken).ConfigureAwait(false);
            if (count <= 0)
                return new PagedList<Customer>([], pageIndex, pageSize, count);

            var list = await query.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync(cancellationToken).ConfigureAwait(false);

            return new PagedList<Customer>(list, pageIndex, pageSize, count);
        }

        public async Task<Customer> InsertAsync(Customer entity, CancellationToken cancellationToken = default)
        {
            await Table.AddAsync(entity, cancellationToken).ConfigureAwait(false);

            if (!Context.Entry(entity).IsKeySet)
                await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return entity;
        }

        public async Task<Customer> UpdateAsync(Customer entity, CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return entity;
        }
    }
}

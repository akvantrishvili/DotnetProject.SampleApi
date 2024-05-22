using DotnetProject.SampleApi.Application.Common;
using Microsoft.EntityFrameworkCore;

namespace DotnetProject.SampleApi.Persistence.Share.Repositories
{
    public abstract class BaseRepository<TDbContext, TEntity>(TDbContext context)
        where TEntity : class
        where TDbContext : DbContext
    {
        protected readonly TDbContext Context = context;
        protected DbSet<TEntity> Table => Context.Set<TEntity>();


        // ReSharper disable once StaticMemberInGenericType
        private static SortingDetails? s_defaultSorting;

        // ReSharper disable once StaticMemberInGenericType
        private static readonly object s_defaultSortingLock = new();

        protected SortingDetails GetDefaultSorting()
        {
            if (s_defaultSorting != null) return s_defaultSorting;

            lock (s_defaultSortingLock)
            {
                var primaryKeys = GetPrimaryKeyFields(typeof(TEntity));
                if (primaryKeys is not { Count: > 0 })
                {
                    s_defaultSorting = new SortingDetails();
                }
                else
                {
                    s_defaultSorting = new SortingDetails(primaryKeys.Select(x => new SortItem(x, SortDirection.Asc)).ToList());
                }
            }
            return s_defaultSorting;
        }

        // ReSharper disable once StaticMemberInGenericType
        private static List<string>? s_primaryKeys;

        // ReSharper disable once StaticMemberInGenericType
        private static readonly object s_primaryKeysLock = new();
        protected List<string> GetPrimaryKeyFields(Type entityType)
        {
            if (s_primaryKeys != null) return s_primaryKeys;
            lock (s_primaryKeysLock)
            {
                s_primaryKeys = Context.Model.FindEntityType(entityType)?.GetKeys()
                                    .FirstOrDefault(x => x.IsPrimaryKey())?.Properties.Select(y => y.Name).ToList()
                                ?? [];
            }
            return s_primaryKeys;
        }
    }
}

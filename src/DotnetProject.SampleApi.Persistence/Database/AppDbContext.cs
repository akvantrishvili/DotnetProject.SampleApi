

using DotnetProject.SampleApi.Domain.Customers;
using Microsoft.EntityFrameworkCore;

namespace DotnetProject.SampleApi.Persistence.Database
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Customer> Customers { get; set; } = null!;

        public DbSet<IdentityDocument> IdentityDocuments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}

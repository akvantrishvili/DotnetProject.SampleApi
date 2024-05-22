// Copyright (C) TBC Bank. All Rights Reserved.
using DotnetProject.SampleApi.Domain.Customers;
using Microsoft.EntityFrameworkCore;

namespace DotnetProject.SampleApi.Persistence.Share.Database
{
    public abstract class BaseAppDbContext<T>(DbContextOptions<T> options) : DbContext(options) where T : DbContext
    {
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<IdentityDocument> IdentityDocuments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Configurations.CustomerConfiguration).Assembly);
        }
    }
}

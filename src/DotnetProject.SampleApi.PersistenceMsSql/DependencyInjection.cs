using System;
using DotnetProject.SampleApi.Application.Contracts;
using DotnetProject.SampleApi.PersistenceMsSql.Database;
using DotnetProject.SampleApi.PersistenceMsSql.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DotnetProject.SampleApi.PersistenceMsSql
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, string databaseConnectionString)
        {
            if (string.IsNullOrWhiteSpace(databaseConnectionString))
                throw new ArgumentNullException(nameof(databaseConnectionString));

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(databaseConnectionString));
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            return services;
        }
    }
}

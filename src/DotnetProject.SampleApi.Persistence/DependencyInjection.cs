
using System;
using DotnetProject.SampleApi.Application.Contracts;
using DotnetProject.SampleApi.Persistence.Database;
using DotnetProject.SampleApi.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace DotnetProject.SampleApi.Persistence
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

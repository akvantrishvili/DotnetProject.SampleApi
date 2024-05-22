// Copyright (C) TBC Bank. All Rights Reserved.

using System;
using DotnetProject.SampleApi.Application.Contracts;

using DotnetProject.SampleApi.PersistencePostgre.Database;
using DotnetProject.SampleApi.PersistencePostgre.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DotnetProject.SampleApi.PersistencePostgre
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistencePostgre(this IServiceCollection services,
            string databaseConnectionString)
        {
            if (string.IsNullOrWhiteSpace(databaseConnectionString))
                throw new ArgumentNullException(nameof(databaseConnectionString));

            services.AddEntityFrameworkNpgsql().AddDbContext<AppDbContext>(options => options.UseNpgsql(databaseConnectionString));
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            return services;
        }
    }
}

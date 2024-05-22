// Copyright (C) TBC Bank. All Rights Reserved.

using DotnetProject.SampleApi.PersistencePostgre.Database;

namespace DotnetProject.SampleApi.PersistencePostgre.Repositories
{
    public class CustomerRepository(AppDbContext context)
        : Persistence.Share.Repositories.CustomerRepository<AppDbContext>(context)
    {
    }
}

using System;
using DotnetProject.SampleApi.Persistence.Share.Database;
using Microsoft.EntityFrameworkCore;

namespace DotnetProject.SampleApi.PersistencePostgre.Database
{
    public class AppDbContext : BaseAppDbContext<AppDbContext>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

    }
}

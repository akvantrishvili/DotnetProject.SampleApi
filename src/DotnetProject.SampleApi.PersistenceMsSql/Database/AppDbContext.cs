using DotnetProject.SampleApi.Persistence.Share.Database;
using Microsoft.EntityFrameworkCore;

namespace DotnetProject.SampleApi.PersistenceMsSql.Database
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : BaseAppDbContext<AppDbContext>(options)
    {

    }
}

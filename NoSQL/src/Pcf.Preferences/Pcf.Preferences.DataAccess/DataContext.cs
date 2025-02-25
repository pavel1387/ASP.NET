using Microsoft.EntityFrameworkCore;
using Pcf.Preferences.Core.Domain;

namespace Pcf.Preferences.DataAccess;

public class DataContext : DbContext
{
    public DataContext()
    {

    }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {

    }

    public DbSet<Preference> Preference { get; set; }
}
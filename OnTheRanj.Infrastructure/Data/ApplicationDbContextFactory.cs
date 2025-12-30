using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OnTheRanj.Infrastructure.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        // Use SQLite for local development/migrations
        var connectionString = "Data Source=/Users/sava446277/Projects/OnTheRanj/OnTheRanj.Infrastructure/Data/OnTheRanj.db";
        optionsBuilder.UseSqlite(connectionString);
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}

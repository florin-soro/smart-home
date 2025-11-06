using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
namespace Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.DatabaseContext
{
    /// <summary>
    /// Used to create the database context for design-time operations like migrations.
    /// </summary>
    public class EnvMeasurementsSqlDbContextFactory : IDesignTimeDbContextFactory<EnvMeasurementsWriteSqlDbContext>
    {
        public EnvMeasurementsWriteSqlDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EnvMeasurementsWriteSqlDbContext>();
            //optionsBuilder.EnableSensitiveDataLogging();
            
            optionsBuilder.UseSqlServer("Server=tcp:whatever.database.windows.net,1433;Initial Catalog=datastore;Persist Security Info=False;User ID=admin543634;Password=whateverfakepwd%&$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;")
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
                    .LogTo(Console.WriteLine, LogLevel.Information)

            return new EnvMeasurementsWriteSqlDbContext(optionsBuilder.Options);
        }
    }
}

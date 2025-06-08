using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace CO.API.Data
{
    public class DbInitializer
    {
        public static void InitializeDatabase(ApiDbContext dbContext, ILogger logger)
        {
            // Step 1: Check if DB exists
            IRelationalDatabaseCreator dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

            if (dbCreator == null)
            {
                throw new InvalidOperationException("Could not get RelationalDatabaseCreator service.");
            }

            if (!dbCreator.Exists())
            {
                logger.LogWarning("Database does not exist. Please run CO.API\\scripts\\instnwnd.sql manually to create it.");
                throw new Exception("Database missing. Manual script execution required.");
            }

            logger.LogInformation("Database exists. Applying EF Core migrations...");

            dbContext.Database.Migrate();

            logger.LogInformation("EF Core migrations applied successfully.");
        }
    }
}

using Serilog;
using CO.API;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .MinimumLevel.Debug()
    .CreateLogger();

try
{
    Log.Information("Starting web host");

    var host = CreateHostBuilder(args).Build();

    var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    if (!string.Equals(envName, "Testing", StringComparison.OrdinalIgnoreCase))
    {
        using (var scope = host.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<CO.API.Data.ApiDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<CO.API.Startup>>();
            CO.API.Data.DbInitializer.InitializeDatabase(dbContext, logger);
        }
    }

    host.Run();
}
catch (Exception ex)
{
    Log.Error(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseSerilog()
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
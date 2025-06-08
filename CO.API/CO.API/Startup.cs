using CO.API.Data;
using CO.API.Handlers;
using CO.API.Middleware;
using Microsoft.EntityFrameworkCore;
using Prometheus;

public class Startup
{

    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });

        services.AddDbContext<ApiDbContext>(options =>
            options.UseSqlServer(Environment.GetEnvironmentVariable(Constants.DefaultConnection) ??
            _configuration.GetConnectionString(Constants.DefaultConnection)));

        services.AddControllers();
        services.AddMemoryCache();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        // handlers here
        services.AddTransient<ICustomerHandler, CustomerHandler>();
    }
    public void Configure(IApplicationBuilder app)
    {
        if (_environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        using (IServiceScope scope = app.ApplicationServices.CreateScope())
        {
            ApiDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
            ILogger<Startup> logger = scope.ServiceProvider.GetRequiredService<ILogger<Startup>>();

            DbInitializer.InitializeDatabase(dbContext, logger);
        }

        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseHttpMetrics();
        app.UseRouting();

        // Use CORS policy
        app.UseCors("AllowAll");

        app.Use(async (context, next) =>
        {
            string requestId = context.TraceIdentifier;
            string ip = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            using (Serilog.Context.LogContext.PushProperty("RequestId", requestId))
            using (Serilog.Context.LogContext.PushProperty("ClientIP", ip))
            {
                await next.Invoke();
            }
        });

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapMetrics();
        });
    }
}


using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Spark.NET.Infrastructure.Services.ApplicationDbContext;

public static class InitializeApplicationDbContextService
{
    public static void RegisterService(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<Contexts.ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));
        /*
         * If using SQL Server, replace use the following line instead:
         * - options.UseSqlServer(connectionString);
         * If using MySQL/MariaDB, replace use the following line instead:
         * - options.UseMySQL(connectionString, serverVersion: ServerVersion.AutoDetect(connectionString)));
         * If using SQLite, replace use the following line instead:
         * - options.UseSqlite(connectionString);
         * If using PostgreSQL, replace use the following line instead:
         * - options.UseNpgsql(connectionString);
        */
    }
}
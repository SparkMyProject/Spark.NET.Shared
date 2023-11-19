using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spark.NET.Shared.Entities.Models.User;

namespace Spark.NET.Infrastructure.Contexts;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public ApplicationDbContext()
    {
        
    }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }

    public static void RegisterService(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SQLiteConnString");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlite(connectionString);
        }, optionsLifetime: ServiceLifetime.Singleton, contextLifetime: ServiceLifetime.Scoped);
        
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
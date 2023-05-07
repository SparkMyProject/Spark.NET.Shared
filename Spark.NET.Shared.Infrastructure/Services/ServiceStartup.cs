using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Spark.NET.Infrastructure.AppSettings.Models;
using Spark.NET.Infrastructure.Services.API;
using Spark.NET.Infrastructure.Services.ApplicationDbContext;
using Spark.NET.Infrastructure.Services.AppSettings;
// using Spark.NET.Infrastructure.Services.Logger;
using Spark.NET.Infrastructure.Services.User;

namespace Spark.NET.Infrastructure.Services;

public class ServiceStartup
{
    private static IConfiguration _appSettingsConfiguration = null!;
    private static ILogger _logger = null!;

    public static IConfiguration RegisterConfiguration(IServiceCollection services, string environment, IConfiguration appSettingsConfiguration = null!)
    {
        _appSettingsConfiguration = appSettingsConfiguration ?? new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables()
            .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(),$"../../Spark.NET.Shared/Spark.NET.Shared.Infrastructure/AppSettings/Configurations/appsettings.{environment}.json"), optional: false)
            .Build();
        Directory.GetCurrentDirectory();
        return _appSettingsConfiguration;
    }
    public static void RegisterServicesDiscovery(IServiceCollection services, params Assembly[] assemblies)
    {
        // services.AddServiced(assemblies);
        // services.AddAutoMapper(assemblies);
    }
    public static void RegisterServices(IServiceCollection services)
    {
        services.AddOptions();
        // Add all of your services here
        Authentication.InitializeJwtService.RegisterService(services, _appSettingsConfiguration);
        API.InitializeApiEndpointsService.RegisterService(services, _appSettingsConfiguration);
        API.InitializeSwaggerService.RegisterService(services, _appSettingsConfiguration);
        InitializeApplicationDbContextService.RegisterService(services, _appSettingsConfiguration);
        UserService.RegisterService(services, _appSettingsConfiguration);
       // LoggerService.RegisterService(services, _appSettingsConfiguration);
    }

    public static void ConfigureSettings(IServiceCollection services)
    {
        InitializeAppSettingsService.RegisterService(services, _appSettingsConfiguration);
        services.Configure<ConnectionStringsSettings>(_appSettingsConfiguration?.GetSection("ConnectionStrings"));
        services.Configure<SecretKeysSettings>(_appSettingsConfiguration?.GetSection("SecretKeys"));
    }

    public static ILogger ConfigureInfrastructureLogger()
    {
        var logger = new LoggerConfiguration()
            .CreateLogger();
        _logger = logger;
        return logger;
    }
}

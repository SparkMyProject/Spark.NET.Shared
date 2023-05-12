using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Spark.NET.Infrastructure.AppSettings.Models;
using Spark.NET.Infrastructure.Services.API;
using Spark.NET.Infrastructure.Services.ApplicationDbContext;
using Spark.NET.Infrastructure.Services.AppSettings;
using Spark.NET.Infrastructure.Services.Authentication;
using Spark.NET.Infrastructure.Services.User;

// using Spark.NET.Infrastructure.Services.Logger;

namespace Spark.NET.Infrastructure.Services;

public class ServiceStartup
{
    public void Run(IServiceCollection services, )
    {
        _environment = environment;
        // Setup AppSettings
        RegisterConfiguration(appSettingsConfiguration);
        // RegisterServicesDiscovery();
        RegisterServices(infrastructureInstance.Services);
        ConfigureSettings();
        ConfigureInfrastructureLogger(infrastructureInstance.Logger);
    }

    public static void RegisterConfiguration(IConfiguration? appSettingsConfiguration = null)
    {
        _appSettingsConfiguration = appSettingsConfiguration ?? new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).Build();
    }
    //
    // public static void RegisterServicesDiscovery(IServiceCollection services, params Assembly[] assemblies)
    // {
    //     // services.AddServiced(assemblies);
    //     // services.AddAutoMapper(assemblies);
    // }

    public void RegisterServices(IServiceCollection services)
    {
        services.AddOptions();
        // Add all of your services here
        InitializeJwtService.RegisterService(services, _appSettingsConfiguration);
        InitializeApiEndpointsService.RegisterService(services, _appSettingsConfiguration);
        InitializeSwaggerService.RegisterService(services, _appSettingsConfiguration);
        InitializeApplicationDbContextService.RegisterService(services, _appSettingsConfiguration);
        UserService.RegisterService(services, _appSettingsConfiguration, logger: _logger);
        // LoggerService.RegisterService(services, _appSettingsConfiguration);
    }

    public void ConfigureSettings()
    {
        InitializeAppSettingsService.RegisterService(_services, _appSettingsConfiguration);
        _services.Configure<ConnectionStringsSettings>(_appSettingsConfiguration?.GetSection("ConnectionStrings"));
        _services.Configure<SecretKeysSettings>(_appSettingsConfiguration?.GetSection("SecretKeys"));
    }

    public void ConfigureInfrastructureLogger(ILogger? logger)
    {
        _logger = logger ?? new LoggerConfiguration()
            .CreateLogger();
    }
}
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
    private static IConfiguration     _appSettingsConfiguration = null!;
    private static ILogger            _logger                   = null!;
    private static string             _environment              = null!;
    private        IServiceCollection _services                 = null!;

    public void Run(IServiceCollection services, string environment,
                                  IConfiguration?   appSettingsConfiguration = null)
    {
        _services = services;
        _environment = environment;
        // Setup AppSettings
        RegisterConfiguration(appSettingsConfiguration);
        // RegisterServicesDiscovery();
        RegisterServices();
        ConfigureSettings();
        ConfigureInfrastructureLogger();
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

    public void RegisterServices()
    {
        _services.AddOptions();
        // Add all of your services here
        InitializeJwtService.RegisterService(_services, _appSettingsConfiguration);
        InitializeApiEndpointsService.RegisterService(_services, _appSettingsConfiguration);
        InitializeSwaggerService.RegisterService(_services, _appSettingsConfiguration);
        InitializeApplicationDbContextService.RegisterService(_services, _appSettingsConfiguration);
        UserService.RegisterService(_services, _appSettingsConfiguration, logger: _logger);
        // LoggerService.RegisterService(services, _appSettingsConfiguration);
    }

    public void ConfigureSettings()
    {
        InitializeAppSettingsService.RegisterService(_services, _appSettingsConfiguration);
        _services.Configure<ConnectionStringsSettings>(_appSettingsConfiguration?.GetSection("ConnectionStrings"));
        _services.Configure<SecretKeysSettings>(_appSettingsConfiguration?.GetSection("SecretKeys"));
    }

    public void ConfigureInfrastructureLogger()
    {
        _logger = new LoggerConfiguration()
            .CreateLogger();
    }
}
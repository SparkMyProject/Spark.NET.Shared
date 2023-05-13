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
    public readonly IServiceCollection Services;
    public readonly string             Environment;
    public readonly IConfiguration     AppSettingsConfiguration;
    public readonly ILogger            Logger;

    public ServiceStartup(IServiceCollection services, string environment, IConfiguration? appSettingsConfiguration = null, ILogger? logger = null)
    {
        Services = services;
        Environment = environment;
        AppSettingsConfiguration = RegisterConfiguration(appSettingsConfiguration);
        Logger = ConfigureInfrastructureLogger(logger);

        RegisterServices(Services);
        ConfigureSettings();
    }

    private IConfiguration RegisterConfiguration(IConfiguration? appSettingsConfiguration)
    {
        return appSettingsConfiguration ?? new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
     .AddEnvironmentVariables()
     .AddJsonFile($"appsettings.{Environment}.json", optional: false)
     .Build();
    }
    //
    // public static void RegisterServicesDiscovery(IServiceCollection services, params Assembly[] assemblies)
    // {
    //     // services.AddServiced(assemblies);
    //     // services.AddAutoMapper(assemblies);
    // }

    private void RegisterServices(IServiceCollection services)
    {
        services.AddOptions();
        // Add all of your services here
        InitializeJwtService.RegisterService(services, AppSettingsConfiguration);
        InitializeApiEndpointsService.RegisterService(services, AppSettingsConfiguration);
        InitializeSwaggerService.RegisterService(services, AppSettingsConfiguration);
        InitializeApplicationDbContextService.RegisterService(services, AppSettingsConfiguration);
        UserService.RegisterService(services, AppSettingsConfiguration, logger: this.Logger);
        // LoggerService.RegisterService(services, _appSettingsConfiguration);
    }

    private void ConfigureSettings()
    {
        InitializeAppSettingsService.RegisterService(Services, AppSettingsConfiguration);
        Services.Configure<ConnectionStringsSettings>(AppSettingsConfiguration?.GetSection("ConnectionStrings"));
        Services.Configure<SecretKeysSettings>(AppSettingsConfiguration?.GetSection("SecretKeys"));
    }

    private ILogger ConfigureInfrastructureLogger(ILogger? logger)
    {
        return logger ?? new LoggerConfiguration().ReadFrom.Configuration(AppSettingsConfiguration).Enrich.FromLogContext().WriteTo.Console().CreateLogger();
                   .CreateLogger();
    }
}
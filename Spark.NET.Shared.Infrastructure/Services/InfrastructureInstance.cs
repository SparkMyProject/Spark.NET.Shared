using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Spark.NET.Infrastructure.AppSettings.Models;
using Spark.NET.Infrastructure.Contexts;
using Spark.NET.Infrastructure.Services.API;
using Spark.NET.Infrastructure.Services.ApplicationDbContext;
using Spark.NET.Infrastructure.Services.AppSettings;
using Spark.NET.Infrastructure.Services.Authentication;
using Spark.NET.Infrastructure.Services.User;

// using Spark.NET.Infrastructure.Services.Logger;

namespace Spark.NET.Infrastructure.Services;

public class InfrastructureInstance
{
    public readonly IServiceCollection Services;
    public readonly string             Environment;
    public readonly IConfiguration     AppSettingsConfiguration;
    public readonly ILogger            Logger; // This is the InfrastructureLogger

    public InfrastructureInstance(IServiceCollection services, string environment, IConfiguration? appSettingsConfiguration = null,
                                  ILogger?           logger = null)
    {
        Services = services;
        Environment = environment;
        AppSettingsConfiguration = RegisterConfiguration(appSettingsConfiguration);
        ConfigureSettings();
        Logger = ConfigureInfrastructureLogger(logger); // You may use the same logger as the API, or a different one. Just pass logger through.

        RegisterServices(Services);
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
        JwtService.RegisterService(services, AppSettingsConfiguration);
        ApplicationDbContext.RegisterService(services, AppSettingsConfiguration);
        ApiServices.RegisterService(services);
        UserService.RegisterService(services);
    }

    private void ConfigureSettings()
    {
        InitializeAppSettingsService.RegisterService(Services, AppSettingsConfiguration);
        Services.Configure<ConnectionStringsSettings>(AppSettingsConfiguration?.GetSection("ConnectionStrings"));
        Services.Configure<SecretKeysSettings>(AppSettingsConfiguration?.GetSection("SecretKeys"));
    }

    private ILogger ConfigureInfrastructureLogger(ILogger? logger)
    {
        return logger ?? new LoggerConfiguration().WriteTo.Console().WriteTo.Seq("http://localhost:5341").CreateLogger();
    }
}
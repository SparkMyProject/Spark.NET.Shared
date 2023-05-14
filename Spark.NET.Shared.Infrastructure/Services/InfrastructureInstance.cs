using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Spark.NET.Infrastructure.AppSettings.Models;
using Spark.NET.Infrastructure.Contexts;
using Spark.NET.Infrastructure.Services.AppSettings;
using Spark.NET.Infrastructure.Services.Authentication;
using Spark.NET.Infrastructure.Services.User;

// using Spark.NET.Infrastructure.Services.Logger;

namespace Spark.NET.Infrastructure.Services;

public class InfrastructureInstance
{
    public readonly IServiceCollection Services;
    public readonly string             Env;
    public readonly IConfiguration     Configuration;
    public readonly ILogger            Logger; // This is the InfrastructureLogger

    public InfrastructureInstance(IServiceCollection services, string environment, IConfiguration? appSettingsConfiguration = null,
                                  ILogger?           logger = null)
    {
        Services = services;
        Env = environment;
        Configuration = RegisterConfiguration(appSettingsConfiguration);
        ConfigureSettings();
        Logger = ConfigureInfrastructureLogger(logger); // You may use the same logger as the API, or a different one. Just pass logger through.
        Logger.Information("InfrastructureInstance created");

        RegisterServices();
    }

    private IConfiguration RegisterConfiguration(IConfiguration? appSettingsConfiguration)
    {
        var x = AppDomain.CurrentDomain.BaseDirectory;
        
        return appSettingsConfiguration ?? new ConfigurationBuilder()
                                                                     .AddEnvironmentVariables()
                                                                     .AddJsonFile(Path.Join(x, $"./AppSettings/Configurations/appsettings.{Env}.json"),
                                                                                  optional: false)
                                                                     .AddJsonFile(Path.Join(x, $"./AppSettings/Configurations/appsettings.global.json"),
                                                                                  optional: false)
                                                                     .Build();
    }
    //
    // public static void RegisterServicesDiscovery(IServiceCollection services, params Assembly[] assemblies)
    // {
    //     // services.AddServiced(assemblies);
    //     // services.AddAutoMapper(assemblies);
    // }

    private void RegisterServices()
    {
        Services.AddOptions();
        // Add all of your services here
        JwtService.RegisterService(Services, Configuration);
        ApplicationDbContext.RegisterService(Services, Configuration);
        UserService.RegisterService(Services);
    }

    private void ConfigureSettings()
    {
        InitializeAppSettingsService.RegisterService(Services, Configuration);
        Services.Configure<ConnectionStringsSettings>(Configuration?.GetSection("ConnectionStrings"));
        Services.Configure<SecretKeysSettings>(Configuration?.GetSection("SecretKeys"));
    }

    private ILogger ConfigureInfrastructureLogger(ILogger? logger)
    {
        return logger ?? new LoggerConfiguration().WriteTo.Console().WriteTo.Seq("http://localhost:5341").CreateLogger();
    }
}
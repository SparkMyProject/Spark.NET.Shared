using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spark.NET.Infrastructure.AppSettings.Models;
using Spark.NET.Infrastructure.Services.API;
using Spark.NET.Infrastructure.Services.AppSettings;

namespace Spark.NET.Infrastructure.Services;

public class ServiceStartup
{
    private static IConfiguration _configuration;

    public static void RegisterConfiguration(IServiceCollection services, string environment)
    {
        _configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables()
            .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(),$"../../Spark.NET.Shared/Spark.NET.Shared.Infrastructure/AppSettings/Configurations/appsettings.{environment}.json"), optional: false)
            .Build();
        Directory.GetCurrentDirectory();
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
        Authentication.InitializeJwtService.RegisterService(services, _configuration);
        API.InitializeApiEndpointsService.RegisterService(services, _configuration);
        API.InitializeSwaggerService.RegisterService(services, _configuration);
    }

    public static void ConfigureSettings(IServiceCollection services)
    {
        InitializeAppSettingsService.RegisterService(services, _configuration);
        services.Configure<ConnectionStringsSettings>(_configuration.GetSection("ConnectionStrings"));
    }
}
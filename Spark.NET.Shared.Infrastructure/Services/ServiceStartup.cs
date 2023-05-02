using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Spark.NET.Infrastructure.Services;

public class ServiceStartup
{
    public static void RegisterServicesDiscovery(IServiceCollection services, params Assembly[] assemblies)
    {
        // services.AddServiced(assemblies);
        // services.AddAutoMapper(assemblies);
    }
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // Add all of your services here
        Authentication.InitializeJwtService.RegisterService(services, configuration);
    }
}
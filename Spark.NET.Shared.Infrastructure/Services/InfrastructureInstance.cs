using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Spark.NET.Infrastructure.Services;

public class InfrastructureInstance
{
    public IServiceCollection Services { get; set; }
    public IConfiguration AppSettingsConfiguration { get; set; }
    public IConfiguration LoggerConfiguration { get; set; }
    public ServiceStartup ServiceStartup { get; set; }
    public string Env { get; set; }

    public void InitializeInfrastructureInstance(IServiceCollection services, string environment, IConfiguration appSettingsConfiguration = null!)
    {
        Services = services;
        ServiceStartup = new ServiceStartup();
        this.Env = environment;
        
        // Adds configuration file - MUST GO FIRST
        AppSettingsConfiguration = ServiceStartup.RegisterConfiguration(services, Env, appSettingsConfiguration);
        // Configures the configuration file (adds it to the service, and objectifies it) - MUST GO SECOND.
        ServiceStartup.ConfigureSettings(services);
        // Adds all services to the service collection
        ServiceStartup.RegisterServices(services);
        services.AddSingleton<InfrastructureInstance>(this);
    }

}
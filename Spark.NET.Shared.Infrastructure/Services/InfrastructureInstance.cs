using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Spark.NET.Infrastructure.Services;

public class InfrastructureInstance
{
    public IServiceCollection Services { get; set; }
    public IConfiguration Configuration { get; set; }
    public ServiceStartup ServiceStartup { get; set; }
    public string Env { get; set; }

    public void InitializeInfrastructureInstance(IServiceCollection services, IConfiguration configuration, string environment)
    {
        Services = services;
        Configuration = configuration;
        ServiceStartup = new ServiceStartup();
        this.Env = environment;
        
        // Adds configuration file - MUST GO FIRST
        ServiceStartup.RegisterConfiguration(services, Env);
        // Configures the configuration file (adds it to the service, and objectifies it) - MUST GO SECOND.
        ServiceStartup.ConfigureSettings(services);
        // Adds all services to the service collection
        ServiceStartup.RegisterServices(services);
        services.AddSingleton<InfrastructureInstance>(this);
    }

}
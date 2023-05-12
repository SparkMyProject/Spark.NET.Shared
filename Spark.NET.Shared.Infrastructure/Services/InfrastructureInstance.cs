using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Spark.NET.Infrastructure.Services;

public class InfrastructureInstance
{
    public IConfiguration     AppSettingsConfiguration { get; set; }
    public ILogger            Logger                   { get; set; }
    public string             Environment              { get; set; }
    public IServiceCollection Services                 { get; set; }

    public InfrastructureInstance(ServiceCollection services, IConfiguration? appSettingsConfiguration, ILogger? logger, string? environment)
    {
        ServiceStartup.Run(services, appSettingsConfiguration, logger, environment);
        AppSettingsConfiguration = appSettingsConfiguration;
        Logger = logger;
        Environment = environment;
        Services = services;
        // May be null, yes.
    }
}
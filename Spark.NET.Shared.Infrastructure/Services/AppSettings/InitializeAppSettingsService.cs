using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Spark.NET.Infrastructure.Services.AppSettings;

public class InitializeAppSettingsService
{
    public static void RegisterService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConfiguration>(configuration);
    }
}
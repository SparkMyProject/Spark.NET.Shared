using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Spark.NET.Infrastructure.Services.API;

public static class InitializeSwaggerService
{
    public static void RegisterService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen();
    }
}
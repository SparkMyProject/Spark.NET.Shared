using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sentry.Serilog;
using Serilog;
using Spark.NET.Infrastructure.AppSettings.Models;
using Spark.NET.Infrastructure.Contexts;
using Spark.NET.Infrastructure.Services.Authentication;
using Spark.NET.Shared.Entities.Models.User;

// using Spark.NET.Infrastructure.Services.Logger;

namespace Spark.NET.Infrastructure.Services;

public class InfrastructureInstance
{
    public readonly IServiceCollection Services;
    public readonly string Env;
    public readonly IConfiguration Configuration;
    public readonly ILogger Logger; // This is the InfrastructureLogger
    public readonly Infrastructure.AppSettings.Models.AppSettings? AppSettings;

    public InfrastructureInstance(IServiceCollection services, string environment, IConfiguration? appSettingsConfiguration = null,
        ILogger? logger = null)
    {
        Services = services;
        Env = environment;
        Configuration = RegisterConfiguration(appSettingsConfiguration);
        Logger = ConfigureInfrastructureLogger(logger); // You may use the same logger as the API, or a different one. Just pass logger through.
        AppSettings = Configuration.Get<Infrastructure.AppSettings.Models.AppSettings>();
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
        Services.AddRazorPages();


        // Add all of your services here
        Services.Configure<Infrastructure.AppSettings.Models.AppSettings>(Configuration);
        Services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

        Services.AddScoped<UserManager<ApplicationUser>>();
        Services.AddScoped<IUserService, UserService>();


        // The services below do not use DI, so they are not registered as scoped services.
        ApplicationDbContext.RegisterService(Services, Configuration);
        AddAuthenticationService.RegisterService(Services, Configuration, AppSettings);


        // configure DI for application services
    }

    private ILogger ConfigureInfrastructureLogger(ILogger? logger)
    {
        var config = Configuration.Get<Infrastructure.AppSettings.Models.AppSettings>();

        return logger ?? new LoggerConfiguration().WriteTo.Console()
            //.WriteTo.Seq("http://localhost:5341",
            //   apiKey: config?.SecretKeys.SeqLoggingSecretKeys.SeqLoggingSecretInfrastructure)
            //  .Enrich.WithProperty("Environment", Env)
            //.Enrich.WithProperty("Project", $"{config?.ProjectName}.Infrastructure")
            .WriteTo.Sentry(x =>
            {
                x.Dsn = config?.SecretKeys.SentryDSN;
                x.Debug = Env == "Development"; // If Env == Development, then enable Debug mode
                x.AutoSessionTracking = true;
                x.EnableTracing = true;
            })
            //.WriteTo.DatadogLogs(config?.SecretKeys.DataDogApiKey)
            .CreateLogger();
    }
}
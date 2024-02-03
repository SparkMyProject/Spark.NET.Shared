using System.Collections.Immutable;
using System.Configuration;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sentry;
using Sentry.Protocol;
using Sentry.Serilog;
using Serilog;
using Spark.NET.Infrastructure.AppSettings.Models;
using Spark.NET.Infrastructure.Contexts;
using Spark.NET.Infrastructure.Services.Authentication;
using Spark.NET.Shared.Entities.Models.User;

// using Spark.NET.Infrastructure.Services.Logger;

namespace Spark.NET.Infrastructure.Services;

public class InfrastructureService
{
    private IConfiguration Configuration { get; }
    private AppSettings.Models.AppSettings? AppSettings { get; }
    private string Env { get; }
    private WebApplicationBuilder Builder { get; }
    private IServiceCollection Services => Builder.Services;
    public InfrastructureService(WebApplicationBuilder builder, string environment)
    {
        Builder = builder;
        Env = environment;
        Configuration = GetConfiguration();
        AppSettings = Configuration.Get<AppSettings.Models.AppSettings>();
    }


    public IConfiguration GetConfiguration()
    {
        var x = AppDomain.CurrentDomain.BaseDirectory;

        var configBuilder = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddJsonFile(Path.Join(x, $"./AppSettings/Configurations/appsettings.{Env}.json"),
                optional: false)
            .AddJsonFile(Path.Join(x, $"./AppSettings/Configurations/appsettings.global.json"),
                optional: false)
            .Build();
        return configBuilder;
    }

    public void RegisterServices()
    {
        // Add all of your services here
        Services.AddControllers();
        Services.AddEndpointsApiExplorer();
        Services.AddSwaggerGen();
        Services.AddOptions();
        Services.AddRazorPages();


        Services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

        Services.AddScoped<UserManager<ApplicationUser>>();
        Services.AddScoped<IUserService, UserService>();

        // Register settings file
        Services.Configure<AppSettings.Models.AppSettings>(Configuration);


        // The services below do not use DI, so they are not registered as scoped services.
        ApplicationDbContext.RegisterService(Services, Configuration);
        JwtService.RegisterService(Services, AppSettings);


        // configure DI for application services
    }

    public void ConfigureLogger()
    {
        var config = Configuration.Get<Infrastructure.AppSettings.Models.AppSettings>();

        // Add Api Logging
        var appSettings = Configuration.Get<Infrastructure.AppSettings.Models.AppSettings>();
        Builder.Host.UseSerilog((ctx, lc) => lc
            .WriteTo.Console()
            .WriteTo.Seq("http://localhost:5341",
                apiKey: appSettings?.SecretKeys.SeqLoggingSecretKeys.SeqLoggingSecretInfrastructure)
            .Enrich.WithProperty("Environment", Env)
            .WriteTo.Sentry(x => // This is for Sentry Error Tracking
            {
                x.Dsn = appSettings?.SecretKeys.SentryDSN;
                x.Debug = Env == "Development"; // If Env == Development, then enable Debug mode
                x.AutoSessionTracking = true;
                x.EnableTracing = true;
            })
            .WriteTo.Sentry(o => // This is for DataDog Error Tracking
            {
                o.Dsn = appSettings?.SecretKeys.SentryDataDogDSN;
                o.BeforeSend = @event =>
                {
                    @event.SetTag("service", appSettings.ProjectName);
                    return @event;
                };
            })
            .WriteTo.DatadogLogs(appSettings?.SecretKeys.DataDogApiKey, service: appSettings.ProjectName)); // This is for DataDog Logging
    }

    public AppSettings.Models.AppSettings GetAppSettings()
    {
        return AppSettings;
    }
}
namespace Spark.NET.Infrastructure.AppSettings.Models;

public class AppSettings
{
    public Logging Logging { get; set; }
    public string SiteName { get; set; }

}

public class Logging
{
    private class LogLevel
    {
        private string Default;
        private string MicrosoftAspNetCore;
    }
}
namespace Spark.NET.Infrastructure.AppSettings.Models;

public class AppSettings
{
    public SecretKeysSettings        SecretKeys        { get; set; }
    public ConnectionStringsSettings ConnectionStrings { get; set; }
    public string                    ProjectName       { get; set; } // This is in global
}

public class ConnectionStringsSettings
{
    public string MySQLConnString        { get; set; }
    public string MicrosoftSQLConnString { get; set; }
    public string PostgresSQLConnString  { get; set; }
}

public class SecretKeysSettings
{
    public string               JwtSecret            { get; set; }
    public SeqLoggingSecretKeys SeqLoggingSecretKeys { get; set; }
    public string               SentryDSN            { get; set; }
    public string               DataDogApiKey       { get; set; }

}

public class SeqLoggingSecretKeys
{
    public string SeqLoggingSecretAPI            { get; set; }
    public string SeqLoggingSecretInfrastructure { get; set; }
    
}
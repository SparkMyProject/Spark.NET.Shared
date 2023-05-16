namespace Spark.NET.Infrastructure.AppSettings.Models;

public class SecretKeysSettings
{
    public string JwtSecret { get; set; }
    public SeqLoggingSecretKeys SeqLoggingSecretKeys { get; set; }
}

public class SeqLoggingSecretKeys
{
    public string SeqLoggingSecretAPI { get; set; }
    public string SeqLoggingSecretInfrastructure { get; set; }
}
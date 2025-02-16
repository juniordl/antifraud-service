namespace AntiFraudService.Infrastructure.Cache;

public class RedisConfiguration
{
    public const string SectionName = "Redis";
    public string Server { get; set; }
}
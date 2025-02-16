using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace AntiFraudService.Infrastructure.Cache;

public class RedisHealthCheck(RedisConfiguration redisConfiguration): IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var redis = await ConnectionMultiplexer.ConnectAsync(redisConfiguration.Server);
            return redis.IsConnected ? HealthCheckResult.Healthy("Redis is reachable") : HealthCheckResult.Unhealthy("Redis is not reachable");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"Redis health check failed: {ex.Message}");
        }
    }
}
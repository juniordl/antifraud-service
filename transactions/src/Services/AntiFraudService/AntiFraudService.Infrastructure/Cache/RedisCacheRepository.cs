using AntiFraudService.Application.Interfaces.Cache;
using StackExchange.Redis;

namespace AntiFraudService.Infrastructure.Cache;

public class RedisCacheRepository: ICacheRepository
{
    private readonly IDatabase _database;

    public RedisCacheRepository(RedisConfiguration configuration)
    {
        var redis = ConnectionMultiplexer.Connect(configuration.Server);
        _database = redis.GetDatabase();
    }

    public async Task SetAsync(string key, double value, TimeSpan expiration)
    {
        await _database.StringSetAsync(key, value, expiration);
    }
    
    public async Task<double?> GetAsync(string key)
    {
        var redisValue = await _database.StringGetAsync(key);
        return redisValue.HasValue ? (double?)redisValue : null;
    }

    public async Task RemoveAsync(string key)
    {
        await _database.KeyDeleteAsync(key);
    }

    public async Task<TimeSpan?> GetTimeToLiveAsync(string key)
    {
        return await _database.KeyTimeToLiveAsync(key);
    }
}
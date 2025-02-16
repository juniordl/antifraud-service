namespace AntiFraudService.Application.Interfaces.Cache;

public interface ICacheRepository
{
    Task SetAsync(string key, double value, TimeSpan expiration);
    Task<double?> GetAsync(string key);
    Task RemoveAsync(string key);
    Task<TimeSpan?> GetTimeToLiveAsync(string key);
}
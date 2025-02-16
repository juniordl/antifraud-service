using AntiFraudService.Application.Events;
using AntiFraudService.Application.Interfaces.Cache;

namespace AntiFraudService.Application.Services;

public class AntiFraudControlService(ICacheRepository cacheRepository)
{
    private const int MaxTransactionValue = 2000;
    private const int MaxTransactionAccumulatedValuePerDay = 20000;
    public async Task<bool> IsApprovedTransaction(TransactionCreatedEvent transaction)
    {
        var isMaxValueExceeded = MaxValueTransactionExceed(transaction);
        var isAccumulatedValueExceeded = await TransactionsAccumulatedValuePerDayExceed(transaction);

        return !isMaxValueExceeded && !isAccumulatedValueExceeded;

    }

    private static bool MaxValueTransactionExceed(TransactionCreatedEvent transaction)
    {
        return transaction.Value > MaxTransactionValue;
    }

    private async Task<bool> TransactionsAccumulatedValuePerDayExceed(TransactionCreatedEvent transaction)
    {
        var accumulated = await cacheRepository.GetAsync(transaction.SourceAccountId.ToString()); 
        if (accumulated is null)
        {
            await cacheRepository.SetAsync(transaction.SourceAccountId.ToString(), transaction.Value, GetRemainingTime());
            return false;
        }
        
        accumulated += transaction.Value;
        if (accumulated > MaxTransactionAccumulatedValuePerDay) 
        {
            return true;
        }

        var ttl = await cacheRepository.GetTimeToLiveAsync(transaction.SourceAccountId.ToString());
        await cacheRepository.SetAsync(transaction.SourceAccountId.ToString(), accumulated.Value, ttl!.Value);
        return false;
    }

    private static TimeSpan GetRemainingTime()
    {
        var now = DateTime.UtcNow;
        var endOfDay = now.Date.AddDays(1);
        return endOfDay - now;
    }
}
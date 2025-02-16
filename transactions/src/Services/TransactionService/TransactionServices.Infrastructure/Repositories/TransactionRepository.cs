using Microsoft.EntityFrameworkCore;
using TransactionService.Domain;
using TransactionServices.Application.Interfaces.Infrastructure.Repositories;
using TransactionServices.Infrastructure.Database;

namespace TransactionServices.Infrastructure.Repositories;

public class TransactionRepository(TransactionDbContext transactionDbContext) : ITransactionRepository
{
    public async Task<List<Transaction>> GetAll()
    {
        return await transactionDbContext.Transaction.ToListAsync();
    }

    public async Task<Transaction> Get(Guid id)
    {
        return await transactionDbContext.Transaction.FindAsync(id) ?? throw new InvalidOperationException();
    }

    public async Task<Transaction> Create(Transaction transaction)
    {
        await transactionDbContext.Transaction.AddAsync(transaction);
        await transactionDbContext.SaveChangesAsync();
        return transaction;
    }

    public async Task<Transaction> Update(Transaction transaction)
    {
        transactionDbContext.Transaction.Update(transaction);
        await transactionDbContext.SaveChangesAsync();
        return transaction;
    }

    public async Task<Transaction> Delete(Transaction transaction)
    {
        transactionDbContext.Transaction.Remove(transaction);
        await transactionDbContext.SaveChangesAsync();
        return transaction;
    }
}
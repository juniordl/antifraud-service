namespace TransactionServices.Application.Interfaces.Infrastructure.Repositories;

public interface ITransactionRepository
{
    public Task<List<TransactionService.Domain.Transaction>> GetAll();
    public Task<TransactionService.Domain.Transaction> Get(Guid id);
    public Task<TransactionService.Domain.Transaction> Create(TransactionService.Domain.Transaction transaction);
    public Task<TransactionService.Domain.Transaction> Update(TransactionService.Domain.Transaction transaction);
    public Task<TransactionService.Domain.Transaction> Delete(TransactionService.Domain.Transaction transaction);
}
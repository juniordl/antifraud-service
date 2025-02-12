using Microsoft.EntityFrameworkCore;
using TransactionService.Domain;
using TransactionServices.Infrastructure.Database.Configurations;

namespace TransactionServices.Infrastructure.Database;

public class TransactionDbContext: DbContext
{
    public DbSet<Transaction> Transaction { get; set; }

    public TransactionDbContext(DbContextOptions<TransactionDbContext> options): base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new TransactionConfiguration());
    }
}
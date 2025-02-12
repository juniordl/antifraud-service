using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain;

namespace TransactionServices.Infrastructure.Database.Configurations;

public class TransactionConfiguration: IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transactions");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.CreatedAt).HasColumnName("createdAt");
        builder.Property(e => e.ModifiedAt).HasColumnName("modifiedAt");
        builder.Property(e => e.SourceAccountId).HasColumnName("sourceAccountId");
        builder.Property(e => e.TransferAccountId).HasColumnName("transferAccountId");
        builder.Property(e => e.TransferType).HasColumnName("transferType");
        builder.Property(e => e.Value).HasColumnName("value");
    }
}
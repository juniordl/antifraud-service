using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TransactionService.Domain;

namespace TransactionServices.Infrastructure.Database.Configurations;

public class TransactionConfiguration: IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transactions");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.Property(e => e.ModifiedAt).HasColumnName("modified_at");
        builder.Property(e => e.SourceAccountId).HasColumnName("source_account_id");
        builder.Property(e => e.TransferAccountId).HasColumnName("transfer_account_id");
        builder.Property(e => e.TransferType).HasColumnName("transfer_type");
        builder.Property(e => e.Value).HasColumnName("value");
        builder.Property(e => e.Status).HasColumnName("status");

        var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
            v => v.Kind == DateTimeKind.Utc ? v : DateTime.SpecifyKind(v, DateTimeKind.Utc),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
        );

        builder.Property(t => t.CreatedAt).HasConversion(dateTimeConverter);
        builder.Property(t => t.ModifiedAt).HasConversion(dateTimeConverter);
    }
}
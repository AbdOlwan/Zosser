using DAL.Entities.Models.PaymentsModels.WalletModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Models.Payments.Wallets
{
    public class WalletTransactionConfiguration : IEntityTypeConfiguration<WalletTransaction>
    {
        public void Configure(EntityTypeBuilder<WalletTransaction> builder)
        {
            builder.ToTable("WalletTransactions");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Amount)
                .IsRequired()
                .HasColumnType("decimal(18, 2)");

            builder.Property(t => t.Type).IsRequired();

            builder.Property(t => t.Description)
                .IsRequired(false) // الوصف قد يكون اختيارياً
                .HasMaxLength(500);

            builder.Property(t => t.SourceType)
                .IsRequired(false)
                .HasMaxLength(50);
        }
    }
}

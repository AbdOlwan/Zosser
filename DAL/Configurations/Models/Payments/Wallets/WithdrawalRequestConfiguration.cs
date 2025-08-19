using DAL.Entities.Models.PaymentsModels.WalletModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Models.Payments.Wallets
{
    public class WithdrawalRequestConfiguration : IEntityTypeConfiguration<WithdrawalRequest>
    {
        public void Configure(EntityTypeBuilder<WithdrawalRequest> builder)
        {
            builder.ToTable("WithdrawalRequests");
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Amount)
                .IsRequired()
                .HasColumnType("decimal(18, 2)");

            builder.Property(r => r.EWalletNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(r => r.WhatsAppNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(r => r.Status).IsRequired();

            builder.Property(r => r.AdminNotes).HasMaxLength(500);

            // علاقة مع المسؤول الذي عالج الطلب
            // إذا تم حذف حساب المسؤول، لا يتم حذف الطلب، بل يصبح الحقل فارغاً
            builder.HasOne(r => r.ProcessedByUser)
                .WithMany() // ApplicationUser ليس له علاقة عكسية مباشرة
                .HasForeignKey(r => r.ProcessedByUserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}

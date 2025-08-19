using DAL.Entities.Models.PaymentsModels.WalletModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Models.Payments.Wallets
{
    public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.ToTable("Wallets");
            builder.HasKey(w => w.Id);

            builder.Property(w => w.Balance)
                .IsRequired()
                .HasColumnType("decimal(18, 2)");

            // علاقة واحد-لواحد مع العميل
            // عند حذف العميل، يتم حذف المحفظة المرتبطة به
            builder.HasOne(w => w.Customer)
                .WithOne(c => c.Wallet) // افترض أنه سيتم إضافة خاصية Wallet إلى Customer
                .HasForeignKey<Wallet>(w => w.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // علاقة واحد-لمتعدد مع حركات المحفظة
            // عند حذف المحفظة، يتم حذف جميع حركاتها
            builder.HasMany(w => w.Transactions)
                .WithOne(t => t.Wallet)
                .HasForeignKey(t => t.WalletId)
                .OnDelete(DeleteBehavior.Cascade);

            // علاقة واحد-لمتعدد مع طلبات السحب
            // عند حذف المحفظة، يتم حذف جميع طلبات السحب الخاصة بها
            builder.HasMany(w => w.WithdrawalRequests)
                .WithOne(r => r.Wallet)
                .HasForeignKey(r => r.WalletId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

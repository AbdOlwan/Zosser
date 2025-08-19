using DAL.Entities.Models.PaymentsModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Models.Payments
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(p => p.Status)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.Method)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.PaymentDate)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(p => p.TransactionId)
                .HasMaxLength(256);



            // خصائص Auditable Entity
            builder.Property(p => p.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(p => p.CreatedBy)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.LastModifiedAt);
            builder.Property(p => p.LastModifiedBy)
                .HasMaxLength(100);

            // الفهارس
            builder.HasIndex(p => p.OrderId).IsUnique().HasDatabaseName("IX_Payment_OrderId_Unique"); // تأكيد أن لكل طلب دفعة واحدة
            builder.HasIndex(p => p.PaymentDate).HasDatabaseName("IX_Payment_PaymentDate");
            builder.HasIndex(p => p.Status).HasDatabaseName("IX_Payment_Status");
            builder.HasIndex(p => p.TransactionId).IsUnique().HasDatabaseName("IX_Payment_TransactionId"); // يجب أن يكون TransactionId فريدًا
        }
    }
}

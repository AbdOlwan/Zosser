using DAL.Entities.Models.PaymentsModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DAL.Configurations.Models.Payments
{
    public class PaymentCollectionConfiguration : IEntityTypeConfiguration<PaymentCollection>
    {
        public void Configure(EntityTypeBuilder<PaymentCollection> builder)
        {
            builder.ToTable("PaymentCollections");
            builder.HasKey(pc => pc.Id);

            builder.Property(pc => pc.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(pc => pc.CollectedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            // Relationship with Payment (One-to-One)
            builder.HasOne(pc => pc.Payment)
                .WithOne(p => p.PaymentCollection)
                .HasForeignKey<PaymentCollection>(pc => pc.PaymentId)
                .OnDelete(DeleteBehavior.Cascade); // إذا حذفت عملية الدفع، تحذف عملية التحصيل

            // Relationship with DeliveryAgent (Many-to-One)
            builder.HasOne(pc => pc.DeliveryAgent)
                .WithMany(da => da.PaymentCollections)
                .HasForeignKey(pc => pc.DeliveryAgentId)
                .OnDelete(DeleteBehavior.Restrict); // منع حذف المندوب إذا كانت هناك تحصيلات مرتبطة به

            // Relationship with User (for VerifiedBy, if applicable)
            // builder.HasOne(pc => pc.VerifiedByUser)
            //     .WithMany()
            //     .HasForeignKey(pc => pc.VerifiedByUserId)
            //     .IsRequired(false)
            //     .OnDelete(DeleteBehavior.SetNull);

            // Auditable properties
            builder.Property(pc => pc.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");
            builder.Property(pc => pc.CreatedBy)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(pc => pc.LastModifiedAt);
            builder.Property(pc => pc.LastModifiedBy)
                .HasMaxLength(100);

            // Indexes
            builder.HasIndex(pc => pc.PaymentId).IsUnique().HasDatabaseName("IX_PaymentCollection_PaymentId_Unique"); // كل دفعة لها تحصيل واحد فقط
            builder.HasIndex(pc => pc.DeliveryAgentId).HasDatabaseName("IX_PaymentCollection_DeliveryAgentId");
            builder.HasIndex(pc => pc.CollectedAt).HasDatabaseName("IX_PaymentCollection_CollectedAt");
        }
    }
}

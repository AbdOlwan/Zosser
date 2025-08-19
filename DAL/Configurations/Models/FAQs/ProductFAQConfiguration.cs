using DAL.Entities.Models.FaqModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Models.FAQs
{
    public class ProductFAQConfiguration : IEntityTypeConfiguration<ProductFAQ>
    {
        public void Configure(EntityTypeBuilder<ProductFAQ> builder)
        {
            builder.ToTable("ProductFAQs");
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Question)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(f => f.Answer)
                .IsRequired()
                .HasMaxLength(2000);

            // تكوين خصائص التتبع
            builder.Property(f => f.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(f => f.CreatedBy)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(f => f.LastModifiedAt);
            builder.Property(f => f.LastModifiedBy)
                .HasMaxLength(100);

            // تكوين العلاقة مع المنتج
            builder.HasOne(f => f.Product)
                .WithMany(p => p.FAQs)
                .HasForeignKey(f => f.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(f => f.ProductId)
                .HasDatabaseName("IX_ProductFAQ_ProductId");
        }
    }
}

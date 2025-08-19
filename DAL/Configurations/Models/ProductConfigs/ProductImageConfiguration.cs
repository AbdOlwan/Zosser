using DAL.Entities.Models.ProductModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Models.ProductConfigs.General
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImages");
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Url)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(i => i.AltText)
                .HasMaxLength(200);

            // العلاقة مع المنتج
            builder.HasOne(i => i.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // فهرس
            builder.HasIndex(i => i.ProductId).HasDatabaseName("IX_ProductImage_ProductId");

            // تتبع التغييرات
            builder.Property(i => i.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(i => i.CreatedBy)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(i => i.LastModifiedAt);
            builder.Property(i => i.LastModifiedBy)
                .HasMaxLength(100);
        }
    }
}
using DAL.Entities.Models.ProductModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Models.ProductConfigs.General
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(p => p.Cashback)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0);

            // خصائص الملابس
            builder.Property(p => p.Material)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.StockQuantity)
                .IsRequired();  

            builder.Property(p => p.CareInstructions).HasMaxLength(500);
            builder.Property(p => p.CountryOfOrigin).HasMaxLength(100);
            builder.Property(p => p.FitType).HasMaxLength(50);
            builder.Property(p => p.TargetGender).HasMaxLength(20);

            // اللون والحجم (واحد فقط لكل منهما)
            builder.Property(p => p.Color)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.Size)
                .IsRequired()
                .HasMaxLength(50);

            // العلاقات
            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Images)
                .WithOne(i => i.Product)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Cascade);



            // فهارس
            builder.HasIndex(p => p.Name).HasDatabaseName("IX_Product_Name");
            builder.HasIndex(p => p.CategoryId).HasDatabaseName("IX_Product_CategoryId");
            builder.HasIndex(p => p.BrandId).HasDatabaseName("IX_Product_BrandId");
            builder.HasIndex(p => p.FitType).HasDatabaseName("IX_Product_FitType");
            builder.HasIndex(p => p.TargetGender).HasDatabaseName("IX_Product_Gender");
            builder.HasIndex(p => p.Color).HasDatabaseName("IX_Product_Color");
            builder.HasIndex(p => p.Size).HasDatabaseName("IX_Product_Size");

            // تتبع التغييرات
            builder.Property(p => p.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(p => p.CreatedBy)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.LastModifiedAt);
            builder.Property(p => p.LastModifiedBy)
                .HasMaxLength(100);
        }
    }
}
using DAL.Entities.Models.ProductModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Models.ProductConfigs.General
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.ToTable("Brands");
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(b => b.LogoUrl)
                .HasMaxLength(500);

            builder.HasIndex(b => b.Name)
                .IsUnique()
                .HasDatabaseName("IX_Brand_Name");

            builder.Property(b => b.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(b => b.CreatedBy)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(b => b.LastModifiedAt);
            builder.Property(b => b.LastModifiedBy)
                .HasMaxLength(100);
        }
    }
}
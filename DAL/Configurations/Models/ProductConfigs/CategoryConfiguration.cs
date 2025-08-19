using DAL.Entities.Models.ProductModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Models.ProductConfigs.General
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.IconUrl)
                .HasMaxLength(500);

            builder.HasIndex(c => c.Name)
                .IsUnique()
                .HasDatabaseName("IX_Category_Name");

            builder.Property(c => c.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(c => c.CreatedBy)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.LastModifiedAt);
            builder.Property(c => c.LastModifiedBy)
                .HasMaxLength(100);
        }
    }
}
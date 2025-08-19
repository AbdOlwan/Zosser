using DAL.Entities.Models.FaqModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Models.FAQs
{
    public class SiteFAQConfiguration : IEntityTypeConfiguration<SiteFAQ>
    {
        public void Configure(EntityTypeBuilder<SiteFAQ> builder)
        {
            builder.ToTable("SiteFAQs");
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

            builder.HasIndex(f => f.Question)
                .HasDatabaseName("IX_SiteFAQ_Question");
        }
    }
}

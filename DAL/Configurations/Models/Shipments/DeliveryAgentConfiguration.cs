using DAL.Entities.Identity;
using DAL.Entities.Models.Shipment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Models.Shipments
{
    public class DeliveryAgentConfiguration : IEntityTypeConfiguration<DeliveryAgent>
    {
        public void Configure(EntityTypeBuilder<DeliveryAgent> builder)
        {
            builder.ToTable("DeliveryAgents");
            builder.HasKey(da => da.Id);

            builder.Property(da => da.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(da => da.PhoneNumber)
                .HasMaxLength(50);

            builder.Property(da => da.Email)
                .HasMaxLength(250);

            builder.Property(da => da.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // If linking to ApplicationUser
            builder.HasOne(da => da.ApplicationUser)
                .WithOne()
                .HasForeignKey<DeliveryAgent>(da => da.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            // Auditable properties
            builder.Property(da => da.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(da => da.CreatedBy)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(da => da.LastModifiedAt);
            builder.Property(da => da.LastModifiedBy)
                .HasMaxLength(100);

            builder.HasIndex(da => da.Name).IsUnique();
            builder.HasIndex(da => da.Email).IsUnique().HasFilter("[Email] IS NOT NULL"); // Unique only if not null
        }
    }
}

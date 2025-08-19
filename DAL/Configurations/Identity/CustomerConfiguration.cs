using DAL.Entities.Identity;
using DAL.Entities.Models.PaymentsModels.WalletModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Models.IdentityConfigs
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");
            builder.HasKey(c => c.Id);

            // العلاقات
            builder.HasOne(c => c.User)
                .WithOne()
                .HasForeignKey<Customer>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);



            builder.HasMany(c=>c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);



            // تتبع التغييرات
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
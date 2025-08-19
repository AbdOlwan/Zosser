using DAL.Entities.Models.OrderModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Models.OrdersConfig
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(o => o.Id);

            // --- Order Financial Properties ---
            // تم حذف Subtotal لأنه لم يعد موجودًا، ويمكن حسابه ضمنيًا من UnitPrice
            builder.Property(o => o.UnitPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(o => o.ShippingCost)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(o => o.CodFee)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(o => o.TotalCashback)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // --- Product Snapshot Properties ---
            builder.Property(o => o.ProductName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(o => o.ProductColor)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(o => o.ProductSize)
                .IsRequired()
                .HasMaxLength(50);


            // --- Order Status and Details ---
            builder.Property(o => o.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(o => o.PaymentMethod)
                .IsRequired();

            builder.Property(o => o.OrderDate)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            // --- Embedded Address Properties ---
            builder.Property(oa => oa.ContactName)
                .IsRequired()
                .HasMaxLength(150);

            // ... (باقي خصائص العنوان كما هي)
            builder.Property(oa => oa.PhoneNumber).IsRequired().HasMaxLength(11);
            builder.Property(oa => oa.AddressLine1).IsRequired().HasMaxLength(200);
            builder.Property(oa => oa.AddressLine2).HasMaxLength(200);
            builder.Property(oa => oa.City).IsRequired().HasMaxLength(100);
            builder.Property(oa => oa.State).IsRequired().HasMaxLength(100);


            // --- Relationships ---
            builder.HasOne(o => o.Customer)
                .WithMany(c => c.Orders) // تحديث: يجب تحديد العلاقة العكسية في Customer
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict); // استخدام Restrict لمنع حذف عميل لديه طلبات


            builder.HasOne(o => o.Product)
                .WithMany(p => p.Orders) // ← تم الإصلاح
                .HasForeignKey(o => o.ProductId);

            // تم حذف العلاقة مع OrderItems لأنها لم تعد موجودة
            // builder.HasMany(o => o.OrderItems)...

            // --- Auditable Entity Properties ---
            builder.Property(o => o.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            // ... (باقي الخصائص аудитаبل كما هي)
            builder.Property(o => o.CreatedBy).IsRequired().HasMaxLength(100);
            builder.Property(o => o.LastModifiedAt);
            builder.Property(o => o.LastModifiedBy).HasMaxLength(100);


            // --- Indexes ---
            builder.HasIndex(o => o.CustomerId).HasDatabaseName("IX_Order_CustomerId");
            builder.HasIndex(o => o.OrderDate).HasDatabaseName("IX_Order_OrderDate");
            builder.HasIndex(o => o.Status).HasDatabaseName("IX_Order_Status");
            builder.HasIndex(o => o.ProductId).HasDatabaseName("IX_Order_ProductId"); // إضافة مفهرس للمنتج
        }
    }
}
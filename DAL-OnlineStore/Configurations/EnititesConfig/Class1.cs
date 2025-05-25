using DAL_OnlineStore.Entities.Models.OrderModels;
using DAL_OnlineStore.Entities.Models.PaymentModels;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL_OnlineStore.Configurations.EntitiesConfig
{
    //public class OrderConfiguration : IEntityTypeConfiguration<Order>
    //{
    //    public void Configure(EntityTypeBuilder<Order> builder)
    //    {
    //        builder.ToTable("Orders");
    //        builder.HasKey(o => o.OrderID);

    //        builder.Property(o => o.OrderDate)
    //               .IsRequired();

    //        builder.Property(o => o.OrderStatus)
    //               .HasConversion<int>()
    //               .IsRequired();

    //        builder.Property(o => o.SubtotalAmount)
    //               .HasColumnType("decimal(18,2)")
    //               .IsRequired();

    //        builder.Property(o => o.TaxAmount)
    //               .HasColumnType("decimal(18,2)");

    //        builder.Property(o => o.ShippingAmount)
    //               .HasColumnType("decimal(18,2)");

    //        builder.Property(o => o.DiscountAmount)
    //               .HasColumnType("decimal(18,2)");

    //        builder.Property(o => o.TotalAmount)
    //               .HasColumnType("decimal(18,2)")
    //               .IsRequired();

    //        builder.Property(o => o.CurrencyRate)
    //               .HasColumnType("decimal(18,6)");

    //        builder.Property(o => o.CurrencyCode)
    //               .HasMaxLength(3)
    //               .IsRequired();

    //        builder.Property(o => o.IsDeleted)
    //               .HasDefaultValue(false);

    //        builder.HasOne(o => o.Customer)
    //               .WithMany(c => c.Orders)
    //               .HasForeignKey(o => o.CustomerID)
    //               .OnDelete(DeleteBehavior.Restrict);

    //        builder.HasOne(o => o.OrderAddress)
    //               .WithOne(a => a.Order)
    //               .HasForeignKey<Order>(o => o.OrderAddressID)
    //               .OnDelete(DeleteBehavior.Cascade);

    //        builder.HasOne(o => o.Payment)
    //               .WithOne(p => p.Order)
    //               .HasForeignKey<Payment>(p => p.OrderID)
    //               .OnDelete(DeleteBehavior.Cascade);

    //        builder.HasOne(o => o.Shipment)
    //               .WithOne(s => s.Order)
    //               .HasForeignKey<Shipment>(s => s.OrderID)
    //               .OnDelete(DeleteBehavior.Cascade);
    //    }
    //}

    //public class OrderAddressConfiguration : IEntityTypeConfiguration<OrderAddress>
    //{
    //    public void Configure(EntityTypeBuilder<OrderAddress> builder)
    //    {
    //        builder.ToTable("OrderAddresses");
    //        builder.HasKey(a => a.OrderAddressID);

    //        builder.Property(a => a.AddressLine1)
    //               .IsRequired();

    //        builder.Property(a => a.City)
    //               .IsRequired();

    //        builder.Property(a => a.State)
    //               .IsRequired();

    //        builder.Property(a => a.Country)
    //               .IsRequired();

    //        builder.HasOne(a => a.Order)
    //               .WithOne(o => o.OrderAddress)
    //               .HasForeignKey<OrderAddress>(a => a.OrderID)
    //               .OnDelete(DeleteBehavior.Cascade);
    //    }
    //}

    //public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    //{
    //    public void Configure(EntityTypeBuilder<OrderItem> builder)
    //    {
    //        builder.ToTable("OrderItems");
    //        builder.HasKey(i => i.OrderItemID);

    //        builder.Property(i => i.Quantity)
    //               .IsRequired();

    //        builder.Property(i => i.UnitPrice)
    //               .HasColumnType("decimal(18,2)")
    //               .IsRequired();

    //        builder.Property(i => i.Subtotal)
    //               .HasColumnType("decimal(18,2)")
    //               .IsRequired();

    //        builder.HasOne(i => i.Order)
    //               .WithMany(o => o.Items)
    //               .HasForeignKey(i => i.OrderID)
    //               .OnDelete(DeleteBehavior.Cascade);

    //        builder.HasOne(i => i.Product)
    //               .WithMany(p => p.OrderItems)
    //               .HasForeignKey(i => i.ProductID)
    //               .OnDelete(DeleteBehavior.Restrict);
    //    }
    //}

    //public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    //{
    //    public void Configure(EntityTypeBuilder<Payment> builder)
    //    {
    //        builder.ToTable("Payments");
    //        builder.HasKey(p => p.PaymentID);

    //        builder.Property(p => p.Amount)
    //               .HasColumnType("decimal(18,2)")
    //               .IsRequired();

    //        builder.Property(p => p.PaymentDate)
    //               .IsRequired();

    //        builder.Property(p => p.Status)
    //               .HasConversion<int>()
    //               .IsRequired();

    //        builder.Property(p => p.Method)
    //               .HasConversion<int>()
    //               .IsRequired();

    //        builder.HasOne(p => p.Order)
    //               .WithOne(o => o.Payment)
    //               .HasForeignKey<Payment>(p => p.OrderID)
    //               .OnDelete(DeleteBehavior.Cascade);
    //    }
    //}

    //public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
    //{
    //    public void Configure(EntityTypeBuilder<Shipment> builder)
    //    {
    //        builder.ToTable("Shipments");
    //        builder.HasKey(s => s.ShipmentID);

    //        builder.Property(s => s.TrackingNumber);

    //        builder.Property(s => s.ShippedDate);
    //        builder.Property(s => s.EstimatedDelivery);
    //        builder.Property(s => s.ActualDelivery);

    //        builder.Property(s => s.Status)
    //               .HasConversion<int>()
    //               .IsRequired();

    //        builder.HasOne(s => s.Order)
    //               .WithOne(o => o.Shipment)
    //               .HasForeignKey<Shipment>(s => s.OrderID)
    //               .OnDelete(DeleteBehavior.Cascade);

    //        builder.HasOne(s => s.Carrier)
    //               .WithMany(c => c.Shipments)
    //               .HasForeignKey(s => s.CarrierID)
    //               .OnDelete(DeleteBehavior.Restrict);
    //    }
    //}
}

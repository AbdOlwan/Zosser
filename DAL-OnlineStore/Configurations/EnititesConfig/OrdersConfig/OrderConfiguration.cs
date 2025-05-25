using DAL_OnlineStore.Entities.Models.OrderModels;
using DAL_OnlineStore.Entities.Models.PaymentModels;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Configurations.EnititesConfig.OrdersConfig
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(o => o.OrderID);

            builder.Property(o => o.OrderDate)
                   .IsRequired();

            builder.Property(o => o.OrderStatus)
                   .HasConversion<int>()
                   .IsRequired();

            builder.Property(o => o.SubtotalAmount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(o => o.TaxAmount)
                   .HasColumnType("decimal(18,2)");

            builder.Property(o => o.ShippingAmount)
                   .HasColumnType("decimal(18,2)");

            builder.Property(o => o.DiscountAmount)
                   .HasColumnType("decimal(18,2)");

            builder.Property(o => o.TotalAmount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();



            builder.Property(o => o.IsDeleted)
                   .HasDefaultValue(false);

            builder.HasOne(o => o.Customer)
                   .WithMany(c => c.orders)
                   .HasForeignKey(o => o.CustomerID)
                   .OnDelete(DeleteBehavior.Restrict);



            builder.HasOne(o => o.Payment)
                   .WithOne(p => p.Order)
                   .HasForeignKey<Payment>(p => p.OrderID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.Shipment)
                   .WithOne(s => s.Order)
                   .HasForeignKey<Shipment>(s => s.OrderID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder
                   .HasOne(o => o.OrderAddress)
                   .WithOne(a => a.Order)
                   .HasForeignKey<OrderAddress>(o => o.OrderAddressID)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

        }
    }

}

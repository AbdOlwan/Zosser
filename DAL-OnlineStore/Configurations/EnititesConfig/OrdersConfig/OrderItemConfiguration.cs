using DAL_OnlineStore.Entities.Models.OrderModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Configurations.EnititesConfig.OrdersConfig
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");
            builder.HasKey(i => i.OrderItemID);

            builder.Property(i => i.Quantity)
                   .IsRequired();

            builder.Property(i => i.UnitPrice)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(i => i.Subtotal)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.HasOne(i => i.Order)
                   .WithMany(o => o.Items)
                   .HasForeignKey(i => i.OrderID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(i => i.Product)
                   .WithMany(p => p.Items)
                   .HasForeignKey(i => i.ProductID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}

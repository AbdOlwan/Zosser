using DAL_OnlineStore.Entities.Models.ShipmentModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Configurations.EnititesConfig.ShipmentsConfig
{
    public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
    {
        public void Configure(EntityTypeBuilder<Shipment> builder)
        {
            builder.ToTable("Shipments");
            builder.HasKey(s => s.ShipmentID);

            builder.Property(s => s.TrackingNumber);

            builder.Property(s => s.ShippedDate);
            builder.Property(s => s.EstimatedDelivery);
            builder.Property(s => s.ActualDelivery);

            builder.Property(s => s.Status)
                   .HasConversion<int>()
                   .IsRequired();

            builder.HasOne(s => s.Order)
                   .WithOne(o => o.Shipment)
                   .HasForeignKey<Shipment>(s => s.OrderID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Carrier)
                   .WithMany(c => c.Shipments)
                   .HasForeignKey(s => s.CarrierID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}

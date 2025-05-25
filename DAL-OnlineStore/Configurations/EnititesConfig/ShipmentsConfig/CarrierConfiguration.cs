using DAL_OnlineStore.Entities.Models.ReviewModels;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Configurations.EnititesConfig.ShipmentsConfig
{
    public class CarrierConfiguration : IEntityTypeConfiguration<Carrier>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Carrier> builder)
        {
            builder.HasKey(a => a.Id);


            builder
                   .Property(a => a.Id)
                   .IsRequired();

            builder.HasMany(c => c.Shipments)
                .WithOne(Shipment => Shipment.Carrier)
                .HasForeignKey(a => a.CarrierID)
                .IsRequired();
        }

    }
}

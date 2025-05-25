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
    public class OrderAddressConfiguration : IEntityTypeConfiguration<OrderAddress>
    {
        public void Configure(EntityTypeBuilder<OrderAddress> builder)
        {
            builder.ToTable("OrderAddresses");
            builder.HasKey(a => a.OrderAddressID);

            builder.Property(a => a.AddressLine1)
                   .IsRequired();

            builder.Property(a => a.City)
                   .IsRequired();

            builder.Property(a => a.State)
                   .IsRequired();

            builder.Property(a => a.Country)
                   .IsRequired();

            builder.HasOne(a => a.Order)
                   .WithOne(o => o.OrderAddress)
                   .HasForeignKey<OrderAddress>(a => a.OrderID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}

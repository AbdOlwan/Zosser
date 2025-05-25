using DAL_OnlineStore.Entities.Models.People;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Configurations.EnititesConfig.PeopleConfig
{
    public class CustomerAddressConfiguration : IEntityTypeConfiguration<CustomerAddress>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<CustomerAddress> builder)
        {
            builder.HasKey(a => a.AddressID);


            builder
                   .Property(a => a.AddressID)
                   .IsRequired();

            builder.HasOne(c => c.customer)
                .WithMany(Customer => Customer.addresses)
                .HasForeignKey(a => a.CustomerID)
                .IsRequired();

            //builder.HasMany(c => c.orders)
            //    .WithOne(Order => Order.CustomerAddress)
            //    .HasForeignKey(a => a.CustomerID)
            //    .IsRequired();









        }

    }
}

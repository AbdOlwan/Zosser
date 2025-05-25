using DAL_OnlineStore.Entities;
using DAL_OnlineStore.Entities.Models.PaymentModels;
using DAL_OnlineStore.Entities.Models.People;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Configurations.EnititesConfig.PeopleConfig
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(a => a.CustomerId);


            builder
                   .Property(a => a.CustomerId)
                   .IsRequired();



            builder.HasMany(c => c.addresses)
                .WithOne(Address => Address.customer)
                .HasForeignKey(a => a.CustomerID)
                .IsRequired();


            //builder
            //.HasOne<AplicationUser>() //  .HasOne(c => c.User) if you have the Navigation Property
            //    .WithOne()
            //    .HasForeignKey<Customer>(c => c.UserID);
            //.OnDelete(DeleteBehavior.Cascade);



            builder
                .HasOne(c => c.Person)
                .WithOne(p => p.Customer)
                .HasForeignKey<Customer>(c => c.PersonId)
                .OnDelete(DeleteBehavior.Cascade);




        }

    }
}

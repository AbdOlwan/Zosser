using DAL_OnlineStore.Entities;
using DAL_OnlineStore.Entities.Models.People;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Configurations.EnititesConfig.PeopleConfig
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> b)
        {
            b.HasKey(p => p.PersonId);

            b.Property(p => p.PhoneNumber)
             .HasMaxLength(11)
             .IsRequired();

            b.HasIndex(p => p.PhoneNumber).IsUnique();

            b.Property(p => p.CreatedAt)
             .HasDefaultValueSql("getutcdate()");



            b.HasOne(p => p.User)
             .WithOne(u => u.Person!)
             .HasForeignKey<ApplicationUser>(u => u.PersonId);

            b.HasOne(p => p.Customer)
             .WithOne(c => c.Person!)
             .HasForeignKey<Customer>(c => c.PersonId);
        }
    }
}

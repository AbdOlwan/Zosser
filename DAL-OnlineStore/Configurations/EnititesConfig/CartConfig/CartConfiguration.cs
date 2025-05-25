using DAL_OnlineStore.Entities;
using DAL_OnlineStore.Entities.Models.CartModels;
using DAL_OnlineStore.Entities.Models.OrderModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Configurations.EnititesConfig.CartConfig
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(a => a.CartId);


            builder
                   .Property(a => a.CartId)
                   .IsRequired();

            builder.Property(d => d.UserID)
                    .IsRequired();


            builder.Property(s => s.CartStatus)
                        .HasConversion<int>();

            builder
                .HasMany(c => c.CartItems)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId);


            builder
                .HasOne<ApplicationUser>()
                .WithOne()
                .HasForeignKey<Cart>(c => c.UserID);

        }

    }
}

using DAL_OnlineStore.Entities.Models.CartModels;
using DAL_OnlineStore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_OnlineStore.Entities.Models.ProductModels;

namespace DAL_OnlineStore.Configurations.EnititesConfig.CartConfig
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<CartItem> builder)
        {
            builder.HasKey(a => a.CartItemId);


            builder
                   .Property(a => a.CartItemId)
                   .IsRequired();

            builder.Property(d => d.CartId)
                    .IsRequired();


            builder.Property(s => s.ProductId)
                       .IsRequired();

            builder
                .HasOne(ci => ci.product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductId);

        }

    }
}

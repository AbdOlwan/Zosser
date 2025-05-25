using DAL_OnlineStore.Entities.Models.People;
using DAL_OnlineStore.Entities.Models.ProductModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Configurations.EnititesConfig.ProductsConfig
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(a => a.ProductId);

            builder
                   .Property(a => a.ProductId)
                   .IsRequired();
        }
    }

    public class ProductTranslationConfiguration : IEntityTypeConfiguration<ProductTranslation>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.
            EntityTypeBuilder<ProductTranslation> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(p => p.ProductName)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(255); 

            builder.Property(p => p.Description)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(255);

            builder.Property(p => p.MarketingSlogan)
               .HasColumnType("NVARCHAR")
               .HasMaxLength(255);

            builder.HasOne(B => B.Product)
                .WithMany(p => p.productTranslations)
                .HasForeignKey(p => p.ProductId);
        }
    }
}

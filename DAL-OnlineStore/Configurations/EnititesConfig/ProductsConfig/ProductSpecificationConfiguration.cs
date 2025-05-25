using DAL_OnlineStore.Entities.Models.ProductModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Configurations.EnititesConfig.ProductsConfig
{
    public class ProductSpecificationConfiguration:IEntityTypeConfiguration<ProductSpecification>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ProductSpecification> builder)
        {
            builder.HasKey(ps => new { ps.ProductId, ps.ValueId }); // تعريف المفتاح الأساسي المركب

            // تكوين العلاقات بين الجداول (إذا لم تكن قد قمت بذلك بالفعل)
            builder
                .HasOne(ps => ps.Product)
                .WithMany(p => p.productSpecifications)
                .HasForeignKey(ps => ps.ProductId);

            builder
                .HasOne(ps => ps.SpecificationValue)
                .WithMany(sv => sv.ProductSpecifications)
                .HasForeignKey(ps => ps.ValueId);
        }
    }
}

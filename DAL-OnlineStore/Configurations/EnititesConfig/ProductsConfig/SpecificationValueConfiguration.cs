using DAL_OnlineStore.Entities.Models.ProductModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Configurations.EnititesConfig.ProductsConfig
{
    public class SpecificationValueConfiguration : IEntityTypeConfiguration<SpecificationValue>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<SpecificationValue> builder)
        {
            builder.HasKey(s => s.Value_id); // تعريف المفتاح الأساسي المركب

            builder.HasOne(sv => sv.specification)
        .WithMany(s => s.SpecificationValues)
        .HasForeignKey(sv => sv.SpecificationId) // تكوين واحد فقط للمفتاح الأجنبي
        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

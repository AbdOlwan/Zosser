using DAL_OnlineStore.Entities.Models.ProductModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Configurations.EnititesConfig.ProductsConfig
{
    public class SpecificationConfiguration : IEntityTypeConfiguration<Specification>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Specification> builder)
        {
            builder.HasKey(s =>  s.SpecificationId); // تعريف المفتاح الأساسي المركب

            builder.Property(s => s.SpecificationId)
                .IsRequired();   
        }
    }

    public class SpecificationTranslationConfiguration : IEntityTypeConfiguration<SpecificationTranslation>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.
            EntityTypeBuilder<SpecificationTranslation> builder)
        {
            builder.HasKey(a => a.Id);

            builder.HasOne(B => B.specification)
                .WithMany(p => p.specificationTranslations)
                .HasForeignKey(p => p.SpecificationId);
        }
    }
}

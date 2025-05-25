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
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Brand> builder)
        {
            builder.HasKey(a => a.Brand_ID);

            builder.HasMany(B => B.Products)
                .WithOne(p => p.brand)
                .HasForeignKey(p => p.Brand_ID);
        }
    }

    public class BrandTranslationConfiguration : IEntityTypeConfiguration<BrandTranslation>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.
            EntityTypeBuilder<BrandTranslation> builder)
        {
            builder.HasKey(a => a.Id);

            builder.HasOne(B => B.Brand)
                .WithMany(p => p.BrandTranslations)
                .HasForeignKey(p => p.Brand_ID);

            // إضافة فلتر افتراضي (اختياري) لجلب culture الحالية
            // هنا نفترض أنك سترسل Culture كخاصية في كل Context instance
            //builder
            //    .HasQueryFilter(pt => pt.Culture == CurrentCulture);
        }
        // خاصية ستضبطها من الـ middleware أو من الـ service extensions
        //public string CurrentCulture { get; set; } = "ar";
    }

}

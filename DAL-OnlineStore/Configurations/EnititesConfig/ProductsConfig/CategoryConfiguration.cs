using DAL_OnlineStore.Entities.Models.ProductModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Configurations.EnititesConfig.ProductsConfig
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(a => a.CategoryID);
        }
    }

    public class CategoryTranslationConfiguration : IEntityTypeConfiguration<CategoryTranslation>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.
            EntityTypeBuilder<CategoryTranslation> builder)
        {
            builder.HasKey(a => a.Id);

            builder.HasOne(B => B.category)
                .WithMany(p => p.Translations)
                .HasForeignKey(p => p.CategoryID);
        }
    }
}

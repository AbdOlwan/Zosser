using DAL_OnlineStore.Entities.Models.ProductModels;
using IdentityModel;
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

            builder.Property(e => e.Slug).HasMaxLength(200);

            builder.HasMany(c => c.Translations)
                  .WithOne(t => t.Category)
                  .HasForeignKey(t => t.CategoryID)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class CategoryTranslationConfiguration : IEntityTypeConfiguration<CategoryTranslation>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.
            EntityTypeBuilder<CategoryTranslation> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Culture).HasMaxLength(10).IsRequired();
            builder.Property(e => e.Category_Name).HasMaxLength(500).IsRequired();

            builder.HasIndex(e => new { e.CategoryID, e.Culture }).IsUnique();
        }
    }
}

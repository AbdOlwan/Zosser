using DAL_OnlineStore.Entities.Models;
using DAL_OnlineStore.Entities.Models.People;
using DAL_OnlineStore.Entities.Models.ProductModels;
using Microsoft.EntityFrameworkCore;


namespace DAL_OnlineStore.Configurations.EnititesConfig.ProductsConfig
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ProductImage> builder)
        {
            builder.HasKey(a => a.ImageID);


            builder
                   .Property(a => a.ImageID)
                   .IsRequired();

            builder.HasOne(c => c.Product)
                .WithMany(product => product.Images)
                .HasForeignKey(a => a.ProductID)
                .IsRequired();











        }

    }
}

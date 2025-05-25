using DAL_OnlineStore.Entities.Models.ProductModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductType = DAL_OnlineStore.Entities.Models.ProductModels.ProductType;

namespace DAL_OnlineStore.Configurations.EnititesConfig.ProductsConfig
{
    public class ProductTypeConfiguration : IEntityTypeConfiguration<ProductType>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ProductType> builder)
        {
            builder.HasKey(T => T.TypeId); // تعريف المفتاح الأساسي المركب


        }
    }
}

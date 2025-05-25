using DAL_OnlineStore.Entities.Models;
using DAL_OnlineStore.Entities.Models.People;
using DAL_OnlineStore.Entities.Models.ReviewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Configurations.EnititesConfig.ReviewsConfig
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(a => a.ReviewID);


            builder
                   .Property(a => a.ReviewID)
                   .IsRequired();

            builder.HasOne(c => c.Product)
                .WithMany(product => product.reviews)
                .HasForeignKey(a => a.ProductID)
                .IsRequired();

            builder.HasOne(c => c.Customer)
                .WithMany(customer => customer.reviews)
                .HasForeignKey(a => a.CustomerID)
                .IsRequired();









        }

    }
}

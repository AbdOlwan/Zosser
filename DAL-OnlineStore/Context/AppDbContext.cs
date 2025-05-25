using DAL_OnlineStore.Entities;
using DAL_OnlineStore.Entities.Models;
using DAL_OnlineStore.Entities.Models.CartModels;
using DAL_OnlineStore.Entities.Models.OrderModels;
using DAL_OnlineStore.Entities.Models.PaymentModels;
using DAL_OnlineStore.Entities.Models.People;
using DAL_OnlineStore.Entities.Models.ProductModels;
using DAL_OnlineStore.Entities.Models.ReviewModels;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using ProductType = DAL_OnlineStore.Entities.Models.ProductModels.ProductType;


namespace DAL_OnlineStore.Context
{
    public class AppDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions options,IOptions<OperationalStoreOptions> operationalStoreOptions)
                              : base(options, operationalStoreOptions)
        {
        }
        //public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        //{}
        // ----------------------------
        // ------ People Models -------
        // ----------------------------
        public DbSet<Person> Persons { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerAddress> CustomerAddresses { get; set; }

        // ----------------------------
        // ------ Order Models --------
        // ----------------------------
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        // ----------------------------
        // ------ Payment Models ------
        // ----------------------------
        public DbSet<Payment> Payments { get; set; }

        // ----------------------------
        // ------ Product Models ------
        // ----------------------------
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductTranslation> ProductTranslations { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<BrandTranslation> BrandTranslations { get; set; } 
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTranslation> CategoriesTranslations { get; set; }
        public DbSet<ProductSpecification> ProductSpecifications { get; set; }
        public DbSet<Specification> Specifications { get; set; }
        public DbSet<SpecificationTranslation> specificationTranslations { get; set; }
        public DbSet<SpecificationValue> SpecificationValues { get; set; }
        public DbSet<ProductType> Types { get; set; }

        // ----------------------------
        // ------ Shipment Models -----
        // ----------------------------
        public DbSet<Carrier> Carriers { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<Review> Reviews { get; set; }

        // ----------------------------
        // ------ Cart Models -----
        // ----------------------------
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            // جميع كيانات الترجمة التي ترث من هذه الواجهة
            var translationTypes = new[]
            {
                 typeof(BrandTranslation),
                 typeof(CategoryTranslation),
                 typeof(ProductTranslation),
                 typeof(SpecificationTranslation),
                 // أضِف ما تحتاج
             };

            foreach (var item in translationTypes)
            {
                builder.Entity(item)
                       .HasKey("Id");   // أو اسم المفتاح الأساسي الصحيح

                // نفترض أن لكلٍ منها خاصية named "Culture"
                builder.Entity(item)
                       .HasQueryFilter(
                           CreateCultureFilterExpression(item)
                       );
            }
        }

        // دالة مساعدة تبني Lambda expression مثل:
        //    (TEntity t) => EF.Property<string>(t, "Culture") == CurrentCulture
        private LambdaExpression CreateCultureFilterExpression(Type entityType)
        {
            var param = Expression.Parameter(entityType, "e");
            var prop = Expression.Call(
                typeof(EF), nameof(EF.Property),
                new[] { typeof(string) },
                param,
                Expression.Constant("Culture")
            );
            var body = Expression.Equal(
                prop,
                Expression.Constant(CurrentCulture)
            );
            return Expression.Lambda(body, param);
        }

        public string CurrentCulture { get; set; } = "ar";

    }
}

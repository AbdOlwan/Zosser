using DAL.Entities.Identity;
using DAL.Entities.Models.FaqModels;
using DAL.Entities.Models.OrderModels;
using DAL.Entities.Models.PaymentsModels;
using DAL.Entities.Models.PaymentsModels.WalletModels;
using DAL.Entities.Models.ProductModels;
using DAL.Entities.Models.Shipment;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;

namespace DAL.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        private readonly ICurrentUserService? _currentUserService;

        // Single constructor for both runtime and design-time
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ICurrentUserService? currentUserService = null)
            : base(options)
        {
            _currentUserService = currentUserService;
        }

        // DbSets
        public DbSet<Customer> Customers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        // -----------------------------------------
        // == Product 
        // ----------------------------------------
        public DbSet<Product> Products { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }


        //--------------------------
        //== Review
        //--------------------------

        //--------------------------
        //== FAQS
        //--------------------------
        public DbSet<SiteFAQ> SiteFAQs { get; set; }
        public DbSet<ProductFAQ> ProductFAQs { get; set; }




        //--------------------------
        //== Order
        //--------------------------
        public DbSet<Order> Orders { get; set; }


        //--------------------------
        //== Payments
        //--------------------------

        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentCollection> PaymentCollections { get; set; }

        //--------------------------
        //== Shipments
        //--------------------------
        public DbSet<DeliveryAgent> DeliveryAgents { get; set; }

        //--------------------------
        //== Wallets
        //--------------------------
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletTransaction> WalletTransactions { get; set; }
        public DbSet<WithdrawalRequest> WithdrawalRequests { get; set; }


        #region Configurations
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            ConfigureIdentityTables(builder);
            ConfigureGlobalSettings(builder);
        }

        private void ConfigureIdentityTables(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>().ToTable("AspNetUsers", "Identity");
            builder.Entity<Microsoft.AspNetCore.Identity.IdentityRole>()
                   .ToTable("AspNetRoles", "Identity");
            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserRole<string>>()
                   .ToTable("AspNetUserRoles", "Identity");
            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserClaim<string>>()
                   .ToTable("AspNetUserClaims", "Identity");
            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserLogin<string>>()
                   .ToTable("AspNetUserLogins", "Identity");
            builder.Entity<Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>>()
                   .ToTable("AspNetRoleClaims", "Identity");
            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserToken<string>>()
                   .ToTable("AspNetUserTokens", "Identity");
        }

        private void ConfigureGlobalSettings(ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(Shared.Interfaces.IAuditableEntity)
                        .IsAssignableFrom(entityType.ClrType))
                {
                    builder.Entity(entityType.ClrType)
                        .Property(nameof(Shared.Interfaces.IAuditableEntity.CreatedAt))
                        .HasDefaultValueSql("GETUTCDATE()");
                    builder.Entity(entityType.ClrType)
                        .Property(nameof(Shared.Interfaces.IAuditableEntity.CreatedBy))
                        .HasDefaultValue("System");
                }
            }

            foreach (var relationship in builder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

        }

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            UpdateAuditableEntities();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            UpdateAuditableEntities();
            return base.SaveChanges();
        }

        private void UpdateAuditableEntities()
        {
            var entries = ChangeTracker.Entries<Shared.Interfaces.IAuditableEntity>();
            var currentTime = DateTime.UtcNow;
            var currentUserId = GetCurrentUserId();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = currentTime;
                        entry.Entity.CreatedBy = currentUserId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedAt = currentTime;
                        entry.Entity.LastModifiedBy = currentUserId;
                        break;
                }
            }
        }

        private string GetCurrentUserId()
        {
            try
            {
                var userId = _currentUserService?.UserId ?? "System";
                return !string.IsNullOrEmpty(userId) ? userId! : "System";
            }
            catch
            {
                return "System";
            }
        }
        #endregion

        #region For Testing

        //    public ApplicationDbContext()
        //    {

        //    }

        //    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        //: base(options)
        //    {
        //    }

        //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //    {
        //        // ده الشرط اللي بيضمن إننا بنعمل Configure بس لو الـ optionsBuilder ما اتعملوش Configure قبل كده
        //        // ده مهم لو الـ DbContext بيتم تهيئته بطرق مختلفة (زي الـ Migrations اللي ممكن تستدعيه بدون options)
        //        if (!optionsBuilder.IsConfigured)
        //        {
        //            // هنا هتحط الـ Connection String بتاعتك مباشرة
        //            // لازم تغير اسم السيرفر وقاعدة البيانات بما يتناسب مع إعداداتك
        //            optionsBuilder.UseSqlServer("Server=db23207.public.databaseasp.net; Database=db23207; User Id=db23207; Password=Q!i8_n7G2=Ts; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;");
        //        }
        //    }

        #endregion
    }
}
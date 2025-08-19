using BLL.Mapping;
using BLL.Services.Implementations;
using BLL.Services.Implementations.FAQs;
using BLL.Services.Implementations.Identity;
using BLL.Services.Implementations.Identity.Dashboard;
using BLL.Services.Implementations.Payments;
using BLL.Services.Implementations.Payments.Wallets;
using BLL.Services.Implementations.ProductServices;
using BLL.Services.Implementations.Shipments;
using BLL.Services.Interfaces;
using BLL.Services.Interfaces.FAQs;
using BLL.Services.Interfaces.Identity;
using BLL.Services.Interfaces.Identity.Dashboard;
using BLL.Services.Interfaces.Payments;
using BLL.Services.Interfaces.Payments.Wallets;
using BLL.Services.Interfaces.ProductInterfaces;
using BLL.Services.Interfaces.Shipments;
using DAL.Context;
using DAL.Entities.Identity;
using DAL.Repositories.Implementations;
using DAL.Repositories.Implementations.FAQs;
using DAL.Repositories.Implementations.Identity;
using DAL.Repositories.Implementations.Orders_Impelementation;
using DAL.Repositories.Implementations.Payments;
using DAL.Repositories.Implementations.Payments.Wallets;
using DAL.Repositories.Implementations.Products_Implementaion;
using DAL.Repositories.Implementations.Shipments;
using DAL.Repositories.Interfaces;
using DAL.Repositories.Interfaces.FAQs;
using DAL.Repositories.Interfaces.Identity;
using DAL.Repositories.Interfaces.Order_Interfaces;
using DAL.Repositories.Interfaces.Payments;
using DAL.Repositories.Interfaces.Payments.Wallets;
using DAL.Repositories.Interfaces.Product_Interfaces;
using DAL.Repositories.Interfaces.Shipments;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shared.Constants;
using Shared.Interfaces;
using Shared.Models;
using System.Text;

namespace API.Services
{
    public static class ServiceExtensions
    {


        public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = false;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            return services;
        }
        public static IServiceCollection ConfigureHttpContext(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            return services;
        }
        public static IServiceCollection ConfigureBusinessServices(this IServiceCollection services)
        {

            #region AddScoped Repositories
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICustomerRepo, CustomerRepo>();
            services.AddScoped<IRefreshTokenRepo, RefreshTokenRepo>();

            // Products
            services.AddScoped<IProductRepo, ProductRepo>();
            services.AddScoped<IBrandRepo, BrandRepo>();
            services.AddScoped<ICategoryRepo, CategoryRepo>();
            services.AddScoped<IProductImageRepo, ProductImageRepo>();
            // Reviews

            // FAQs
            services.AddScoped<ISiteFAQRepo, SiteFAQRepo>();
            services.AddScoped<IProductFAQRepo, ProductFAQRepo>();



            // Orders
            services.AddScoped<IOrderRepo, OrderRepo>();

            // Payments
            services.AddScoped<IPaymentRepo, PaymentRepo>();
            services.AddScoped<IPaymentCollectionRepo, PaymentCollectionRepo>();

            // Shipments
            services.AddScoped<IDeliveryAgentRepo, DeliveryAgentRepo>();

            // Wallets
            services.AddScoped<IWalletRepo, WalletRepo>();
            services.AddScoped<IWalletTransactionRepo, WalletTransactionRepo>();
            services.AddScoped<IWithdrawalRequestRepo, WithdrawalRequestRepo>();

            #endregion


            #region AddScoped Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IUserService, UserService>();

            // Products
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IProductImageService, ProductImageService>();


            // FAQs
            services.AddScoped<ISiteFAQService, SiteFAQService>();
            services.AddScoped<IProductFAQService, ProductFAQService>();



            // Orders
            services.AddScoped<IOrderService, OrderService>();

            // Payments
            services.AddScoped<IPaymentService, PaymentService>();
           // services.AddScoped<IPaymentCollectionService, PaymentCollectionService>();

            // Shipments
            services.AddScoped<IDeliveryAgentService, DeliveryAgentService>();

            // Wallets
            services.AddScoped<IWalletService, WalletService>();
            #endregion
            return services; 
        }

        public static IServiceCollection ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
            return services;
        }

        // تعديل جميع الدوال لترجع IServiceCollection
        public static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }
      //  services.AddScoped<ICarrierRepo, CarrierRepo>();

        public static IServiceCollection ConfigureMapster(this IServiceCollection services)
        {
            // تنظيم إعدادات Mapster
            MapsterConfiguration.ConfigureMappings();

            // تسجيل Mapper كخدمة Singleton
            var config = TypeAdapterConfig.GlobalSettings;
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }

     

        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "Customer Management API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });
            });

            return services;
        }


        public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // إعدادات JWT
            var jwtSettings = configuration.GetSection("Jwt");
            var jwtKey = jwtSettings["Key"];
            if (jwtKey == null) return services;
            

                var key = Encoding.UTF8.GetBytes(jwtKey);

                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidateAudience = true,
                        ValidAudience = jwtSettings["Audience"],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

                return services;
            }
        
        

        public static IServiceCollection ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                // سياسات التصريح
                options.AddPolicy(Policies.RequireAdminRole, policy =>
                    policy.RequireRole(Roles.Admin));

                options.AddPolicy(Policies.RequireCustomerRole, policy =>
                    policy.RequireRole(Roles.Customer));


                // سياسة خاصة بالمدراء والمشرفين (يمكن توسيعها)
                options.AddPolicy("RequireManagerAccess", policy =>
                    policy.RequireRole(Roles.Admin, Roles.Manager));

                // سياسة للموظفين أيضاً
                options.AddPolicy("RequireEmployeeAccess", policy =>
                    policy.RequireRole(Roles.Admin, Roles.Manager, Roles.Employee));
            });

            return services;
        }
    }
}

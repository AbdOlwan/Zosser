using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs;
using BLL_OnlineStore.Interfaces;
using BLL_OnlineStore.Interfaces.CartBusServices;
using BLL_OnlineStore.Interfaces.OrderBusServices;
using BLL_OnlineStore.Interfaces.PaymentBusServices;
using BLL_OnlineStore.Interfaces.PeopleBusServices;
using BLL_OnlineStore.Interfaces.ProductBusServices;
using BLL_OnlineStore.Interfaces.ReviewBusServices;
using BLL_OnlineStore.Interfaces.ShipmentBusServices;
using BLL_OnlineStore.Services;
using BLL_OnlineStore.Services.CartBusServices;
using BLL_OnlineStore.Services.CartItemBusServices;
using BLL_OnlineStore.Services.Interfaces;
using BLL_OnlineStore.Services.OrderBusServices;
using BLL_OnlineStore.Services.PaymentBusServices;
using BLL_OnlineStore.Services.PeopleBusServices;
using BLL_OnlineStore.Services.ProductBusServices;
using BLL_OnlineStore.Services.ReviewBusServices;
using BLL_OnlineStore.Services.ShipmentBusServices;
using DAL_OnlineStore.Configurations.Config;
using DAL_OnlineStore.Context;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using DAL_OnlineStore.Repositories;
using DAL_OnlineStore.Repositories.Implementations;
using DAL_OnlineStore.Repositories.Implementations.CartItemRepository;
using DAL_OnlineStore.Repositories.Implementations.CartRepository;
using DAL_OnlineStore.Repositories.Implementations.OrderRepository;
using DAL_OnlineStore.Repositories.Implementations.PaymentRepository;
using DAL_OnlineStore.Repositories.Implementations.PeopleRepository;
using DAL_OnlineStore.Repositories.Implementations.ProductRepository;
using DAL_OnlineStore.Repositories.Implementations.ReviewRepository;
using DAL_OnlineStore.Repositories.Implementations.ShipmentRepository;
using DAL_OnlineStore.Repositories.Interfaces;
using DAL_OnlineStore.Repositories.Interfaces.CartItemRepository;
using DAL_OnlineStore.Repositories.Interfaces.CartRepository;
using DAL_OnlineStore.Repositories.Interfaces.OrderRepository;
using DAL_OnlineStore.Repositories.Interfaces.PaymentRepository;
using DAL_OnlineStore.Repositories.Interfaces.PeopleRepository;
using DAL_OnlineStore.Repositories.Interfaces.ProductRepository;
using DAL_OnlineStore.Repositories.Interfaces.ReviewRepository;
using DAL_OnlineStore.Repositories.Interfaces.ShipmentRepository;
using DAL_OnlineStore.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API_OnlineStore.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // تسجيل DbContext مع قاعدة البيانات باستخدام EF Core
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // تسجيل ASP.NET Core Identity لاستخدام إدارة المستخدمين والأدوار
            services.AddIdentity<DAL_OnlineStore.Entities.ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(opts =>
            {
                // لا تجبر المستخدم على إدخال Email
                opts.User.RequireUniqueEmail = false;
                // (اختياري) لو عايز تطلب التأكيد على الموبايل بدل الإيميل:
                opts.SignIn.RequireConfirmedEmail = false;
            });

            // تسجيل الـ Repositories

            services.AddScoped<ICarrierRepo, CarrierRepo>();
            services.AddScoped<ICustomerAddressRepo, CustomerAddressRepo>();
            services.AddScoped<ICustomerRepo, CustomerRepo>();
            services.AddScoped<IOrderItemRepo, OrderItemRepo>();
            services.AddScoped<IOrderRepo, OrderRepo>();
            services.AddScoped<IPaymentRepo, PaymentRepo>();
            services.AddScoped<IProductImageRepo, ProductImageRepo>();
            services.AddScoped<IProductRepo, ProductRepo>();
            services.AddScoped<IReviewRepo, ReviewRepo>();
            services.AddScoped<IShipmentRepo, ShipmentRepo>();
            //+++
            services.AddScoped<IBrandRepo, BrandRepo>();
            services.AddScoped<ICategoryRepo, CategoryRepo>();
            services.AddScoped<IProductSpecificationRepo, ProductSpecificationRepo>();
            services.AddScoped<ISpecificationRepo, SpecificationRepo>();
            services.AddScoped<ISpecificationValueRepo, SpecificationValueRepo>();
            services.AddScoped<ITypeRepo, TypeRepo>();
            services.AddScoped<IPersonRepo, PersonRepo>();
            services.AddScoped<ICartRepo, CartRepo>();
            services.AddScoped<ICartItemRepo, CartItemRepo>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // تسجيل الخدمات الخاصة بطبقة الـ BLL
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICarrierServices, CarrierServices>();
            services.AddScoped<ICustomerAddressServices, CustomerAddressServices>();
            services.AddScoped<ICustomerServices, CustomerServices>();
            services.AddScoped<IOrderItemService, OrderItemService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IProductImageServices, ProductImageServices>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IReviewServices, ReviewServices>();
            services.AddScoped<IShipmentService, ShipmentService>();
            //+++
            services.AddScoped<IBrandServices, BrandServices>();
            services.AddScoped<ICategoryServices, CategoryServices>();
            services.AddScoped<IProductSpecificationServices, ProductSpecificationServices>();
            services.AddScoped<ISpecificationServices, SpecificationServices>();
            services.AddScoped<ISpecificationValueServices, SpecificationValueServices>();
            services.AddScoped<ITypeServices, TypeServices>();
            services.AddScoped<IPersonServices, personServices>();
            services.AddScoped<ICartServices, CartServices>();
            services.AddScoped<ICartItemServices, CartItemServices>();
            services.AddScoped<ICultureService, CultureService>();


           // services.AddAutoMapper(typeof(Program));

            // تكوين إعدادات الـ JWT من ملف الإعدادات (appsettings.json)
            var jwtSettings = configuration.GetSection("JWT");
            services.Configure<JWT>(jwtSettings);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CanEditPolicy", policy =>
                    policy.RequireClaim("can_edit", "true")); // Only users with this claim can access
            });

            // تسجيل إعدادات الـ Authentication باستخدام JWT
            var secretKey = jwtSettings["Key"];
            if (secretKey != null)
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    // في بيئة التطوير يمكن تعطيل HTTPSMetadata، ولكن يجب تمكينه في البيئات الحية
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                    };
                });
            }
            }

        public static IApplicationBuilder UseRequestCulture(this IApplicationBuilder app)
        {
            return app.Use(async (context, next) =>
            {
                // مثال: ?culture=en أو من الهيدر Accept-Language
                var culture = context.Request.Query["culture"].FirstOrDefault()
                              ?? context.Request.Headers["Accept-Language"].ToString().Split(',').FirstOrDefault()
                              ?? "ar";

                // نخزنها في خدمة الثقافة
                var cultureService = context.RequestServices.GetRequiredService<ICultureService>();
                cultureService.SetCulture(culture);

                await next();
            });
        }

    }
}

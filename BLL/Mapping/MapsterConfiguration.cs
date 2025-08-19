using DAL.Entities.Identity;
using DAL.Entities.Models.FaqModels;
using DAL.Entities.Models.OrderModels;
using DAL.Entities.Models.PaymentsModels;
using DAL.Entities.Models.PaymentsModels.WalletModels;
using DAL.Entities.Models.ProductModels;
using DAL.Entities.Models.Shipment;
using Mapster;
using Shared.DTOs.CustomerDtos;
using Shared.DTOs.Dashboard;
using Shared.DTOs.FAQs;
using Shared.DTOs.IdentityDtos;
using Shared.DTOs.Order;
using Shared.DTOs.Payments;
using Shared.DTOs.Payments.Wallets;
using Shared.DTOs.ProductDTOs;
using Shared.DTOs.ProductDTOs.General;
using Shared.DTOs.Shipments;

namespace BLL.Mapping
{
    public static class MapsterConfiguration
    {
        public static void ConfigureMappings()
        {



            TypeAdapterConfig<ApplicationUser, UserSummaryDto>
                .NewConfig()
                .Map(dest => dest.UserId, src => src.Id)
                .Map(dest => dest.FullName, src => src.Name);
            // Other properties will map by name convention

            TypeAdapterConfig<ApplicationUser, UserDetailsDto>
                .NewConfig()
                .Map(dest => dest.UserId, src => src.Id)
                .Map(dest => dest.FullName, src => src.Name);



            // ===== NEW: Identity Mappings =====
            TypeAdapterConfig<RegisterRequestDto, ApplicationUser>
                .NewConfig()
                .Map(dest => dest.Name, src => src.FullName) // Map FullName from DTO to Name in Entity
                .Map(dest => dest.UserName, src => src.PhoneNumber); // Critical: Set UserName to PhoneNumber for Identity


            // ===== 1. Customer & User Mappings =====
            TypeAdapterConfig<ApplicationUser, CustomerDto>
                .NewConfig()
                .Map(dest => dest.UserId, src => src.Id)
                .Map(dest => dest.FullName, src => src.Name ?? string.Empty)
                .Map(dest => dest.Email, src => src.Email ?? string.Empty)
                .Map(dest => dest.PhoneNumber, src => src.PhoneNumber ?? string.Empty)
                .Map(dest => dest.Gender, src => src.Gender ?? string.Empty)
                .Map(dest => dest.DateOfBirth, src => src.DateOfBirth)
                .Map(dest => dest.IsActive, src => src.IsActive)
                .Map(dest => dest.RegistrationDate, src => src.RegistrationDate)
                .Map(dest => dest.CreatedAt, src => src.CreatedAt)
                .Map(dest => dest.CreatedBy, src => src.CreatedBy ?? string.Empty)
                .Map(dest => dest.LastModifiedAt, src => src.LastModifiedAt)
                .Map(dest => dest.LastModifiedBy, src => src.LastModifiedBy ?? string.Empty)
                .AfterMapping((src, dest) =>
                {
                    if (src.Customer != null) dest.Id = src.Customer.Id;
                });

            TypeAdapterConfig<Customer, CustomerSummaryDto>
                .NewConfig()
                .Map(dest => dest.FullName, src => src.User != null ? src.User.Name : string.Empty)
                .Map(dest => dest.Email, src => src.User != null ? src.User.Email : string.Empty)
                .Map(dest => dest.PhoneNumber, src => src.User != null ? src.User.PhoneNumber : string.Empty)
                .Map(dest => dest.IsActive, src => src.User != null && src.User.IsActive)
                .Map(dest => dest.RegistrationDate, src => src.User != null ? src.User.RegistrationDate : default);




            // ===== 2. Brand Mappings =====
            TypeAdapterConfig<Brand, BrandResponseDTO>
                .NewConfig()
                .Map(dest => dest.ProductCount, src => src.Products.Count);

            TypeAdapterConfig<BrandCreateDTO, Brand>
                .NewConfig()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.Products)
                .Ignore(dest => dest.CreatedAt)
                .Ignore(dest => dest.CreatedBy)
                .Ignore(dest => dest.LastModifiedAt!)
                .Ignore(dest => dest.LastModifiedBy!);

            TypeAdapterConfig<BrandUpdateDTO, Brand>
                .NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Ignore(dest => dest.Products)
                .Ignore(dest => dest.CreatedAt)
                .Ignore(dest => dest.CreatedBy)
                .IgnoreNullValues(true);

            // ===== 3. Category Mappings =====
            TypeAdapterConfig<Category, CategoryResponseDTO>
                .NewConfig()
                .Map(dest => dest.ProductCount, src => src.Products.Count);

            TypeAdapterConfig<CategoryCreateDTO, Category>
                .NewConfig()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.Products)
                .Ignore(dest => dest.CreatedAt)
                .Ignore(dest => dest.CreatedBy)
                .Ignore(dest => dest.LastModifiedAt!)
                .Ignore(dest => dest.LastModifiedBy!);

            // ===== 4. Product Mappings =====
            TypeAdapterConfig<Product, ProductResponseDTO>
                .NewConfig()
                .Map(dest => dest.CategoryName, src => src.Category.Name)
                .Map(dest => dest.BrandName, src => src.Brand.Name)
                .Map(dest => dest.Images, src => src.Images.Adapt<List<ProductImageResponseDTO>>());

            TypeAdapterConfig<Product, ProductCardDto>
                .NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Price, src => src.Price)
                .Map(dest => dest.CashbackAmount, src => src.Cashback)

                .Map(dest => dest.PrimaryImageUrl, src => GetMainImageUrl(src))
                .Map(dest => dest.BrandName, src => src.Brand.Name);

            // ===== 5. ProductImage Mappings =====
            TypeAdapterConfig<ProductImage, ProductImageResponseDTO>
                .NewConfig();


            // ===== 6. FAQ Mappings =====
            TypeAdapterConfig<BaseFAQ, BaseFAQReadDto>
                .NewConfig();

            TypeAdapterConfig<ProductFAQ, ProductFAQReadDto>
                .NewConfig();

            TypeAdapterConfig<ProductFAQCreateDto, ProductFAQ>
                .NewConfig()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.Product)
                .Ignore(dest => dest.CreatedAt)
                .Ignore(dest => dest.CreatedBy);



            // ===== 7. Order Mappings =====

            TypeAdapterConfig<Order, OrderDto>.NewConfig();

            TypeAdapterConfig<Order, OrderDetailsDto>
                .NewConfig()
                 .Map(dest => dest.Customer, src => src.Customer);

            TypeAdapterConfig<OrderForCreationDto, Order>
                .NewConfig()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.Status)
                .Ignore(dest => dest.OrderDate)
                .Ignore(dest => dest.CreatedAt)
                .Ignore(dest => dest.CreatedBy)
                .Ignore(dest => dest.LastModifiedAt)
                .Ignore(dest => dest.LastModifiedBy)
                .Ignore(dest => dest.Customer) // تجاهل خصائص التنقل
                .Ignore(dest => dest.Product);  // تجاهل خصائص التنقل


            TypeAdapterConfig<OrderForUpdateDto, Order>
                .NewConfig()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.CustomerId)
                .Ignore(dest => dest.ProductId)
                .Ignore(dest => dest.ProductName)
                .Ignore(dest => dest.ProductColor)
                .Ignore(dest => dest.ProductSize)
                .Ignore(dest => dest.UnitPrice)
                .Ignore(dest => dest.ShippingCost)
                .Ignore(dest => dest.CodFee)
                .Ignore(dest => dest.TotalAmount)
                .Ignore(dest => dest.TotalCashback)
                .Ignore(dest => dest.PaymentMethod)
                .Ignore(dest => dest.OrderDate)
                .Ignore(dest => dest.CreatedAt)
                .Ignore(dest => dest.CreatedBy)
                .Ignore(dest => dest.LastModifiedAt)
                .Ignore(dest => dest.LastModifiedBy);

            // ===== 8.  Payment Mappings =====
            TypeAdapterConfig<Payment, PaymentResponseDTO>
                .NewConfig();

            TypeAdapterConfig<PaymentCreateDTO, Payment>
                .NewConfig()
                .Map(dest => dest.Status, src => PaymentStatus.Pending); // Payments always start as Pending

            // ===== 9.  Payment Collection Mappings =====
            TypeAdapterConfig<PaymentCollectionCreateDTO, PaymentCollection>
                .NewConfig();

            TypeAdapterConfig<PaymentCollection, PaymentCollectionResponseDTO>
                .NewConfig()
                .Map(dest => dest.OrderId, src => src.Payment.OrderId)
                .Map(dest => dest.PaymentAmount, src => src.Payment.Amount)
                .Map(dest => dest.CollectedAmount, src => src.Amount)
                // Assumption: DeliveryAgent has a navigation property to the ApplicationUser entity to get user details.
                // Your repository should use .Include(pc => pc.DeliveryAgent).ThenInclude(da => da.User) for this to work.
                .Map(dest => dest.DeliveryAgentName, src => src.DeliveryAgent.ApplicationUser!.Name)
                .Map(dest => dest.DeliveryAgentPhone, src => src.DeliveryAgent.ApplicationUser!.PhoneNumber)
                .Map(dest => dest.DeliveryAgentEmail, src => src.DeliveryAgent.ApplicationUser!.Email);


            // ===== 10. Delivery Agent Mappings =====
            TypeAdapterConfig<DeliveryAgentCreateDTO, DeliveryAgent>
                .NewConfig()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.IsActive, src => src.IsActive)
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.UserId!)
                .Ignore(dest => dest.ApplicationUser!)
                .Ignore(dest => dest.PaymentCollections)
                .Ignore(dest => dest.CreatedAt)
                .Ignore(dest => dest.CreatedBy);
  

            TypeAdapterConfig<DeliveryAgentUpdateDTO, DeliveryAgent>
                .NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name, src => !string.IsNullOrEmpty(src.Name))
                .Map(dest => dest.PhoneNumber, src => src.PhoneNumber, src => !string.IsNullOrEmpty(src.PhoneNumber))
                .Map(dest => dest.Email, src => src.Email, src => !string.IsNullOrEmpty(src.Email))
                .Map(dest => dest.IsActive, src => src.IsActive, src => src.IsActive.HasValue)
                .Ignore(dest => dest.UserId!)
                .Ignore(dest => dest.ApplicationUser!)
                .Ignore(dest => dest.PaymentCollections)
                .Ignore(dest => dest.CreatedAt)
                .Ignore(dest => dest.CreatedBy);


            TypeAdapterConfig<DeliveryAgent, DeliveryAgentResponseDTO>
                .NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.IsActive, src => src.IsActive)
                .Map(dest => dest.CreatedAt, src => src.CreatedAt)
                .Map(dest => dest.CreatedBy, src => src.CreatedBy)
                .Map(dest => dest.LastModifiedAt, src => src.LastModifiedAt)
                .Map(dest => dest.LastModifiedBy, src => src.LastModifiedBy)
                .Map(dest => dest.TotalCollections, src => src.PaymentCollections.Count);

            TypeAdapterConfig<DeliveryAgent, DeliveryAgentCardDTO>
                .NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
                .Map(dest => dest.IsActive, src => src.IsActive)
                .Map(dest => dest.TotalCollections, src => src.PaymentCollections.Count)
                // ActiveCollections سيتم تعبئتها يدوياً في الخدمة (Service)
                .Ignore(dest => dest.ActiveCollections);


            // ===== 11. Wallet Mappings (NEW) =====

            // Wallet -> WalletResponseDto
            TypeAdapterConfig<Wallet, WalletResponseDto>
                .NewConfig()
                .Map(dest => dest.CustomerName, src => src.Customer.User.Name);

            // WalletTransaction -> WalletTransactionResponseDto
            TypeAdapterConfig<WalletTransaction, WalletTransactionResponseDto>
                .NewConfig()
                .Map(dest => dest.Type, src => src.Type.ToString()); // تحويل الـ enum إلى string

            // WithdrawalRequestCreateDto -> WithdrawalRequest
            TypeAdapterConfig<WithdrawalRequestCreateDto, WithdrawalRequest>
                .NewConfig()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.WalletId)
                .Ignore(dest => dest.Wallet)
                .Ignore(dest => dest.Status)
                .Ignore(dest => dest.RequestDate)
                .Ignore(dest => dest.LastUpdateDate!)
                .Ignore(dest => dest.ProcessedByUserId!)
                .Ignore(dest => dest.ProcessedByUser!)
                .Ignore(dest => dest.AdminNotes!);

            // WithdrawalRequest -> WithdrawalRequestResponseDto
            TypeAdapterConfig<WithdrawalRequest, WithdrawalRequestResponseDto>
                .NewConfig()
                .Map(dest => dest.ProcessedByUserName,
                     src => src.ProcessedByUser != null ? src.ProcessedByUser.Name : null);

            // Global Settings
            TypeAdapterConfig.GlobalSettings.Default.IgnoreNullValues(true);
            TypeAdapterConfig.GlobalSettings.Default.MapToConstructor(true);
        }

        private static string GetMainImageUrl(Product product)
        {
            if (product.Images == null || !product.Images.Any())
                return string.Empty;

            var primaryImage = product.Images.FirstOrDefault(i => i.IsPrimary);
            return primaryImage?.Url ?? product.Images.First().Url;
        }
    }
}
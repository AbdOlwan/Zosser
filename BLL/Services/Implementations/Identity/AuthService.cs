using BLL.Services.Interfaces.Identity;
using DAL.Entities.Identity;
using DAL.Entities.Models.PaymentsModels.WalletModels;
using DAL.Repositories.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Shared.Constants;
using Shared.DTOs.IdentityDtos;
using Shared.Interfaces;
using Shared.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BLL.Services.Implementations.Identity
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            ILogger<AuthService> logger
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<AuthModel> RegisterAsync(RegisterRequestDto registerDto)
        {
            // The check for an existing user is implicitly handled by _userManager.CreateAsync,
            // which provides more detailed errors.
            var existingUser = await _userManager.FindByNameAsync(registerDto.PhoneNumber);
            if (existingUser != null)
            {
                throw new InvalidOperationException(ApiMessages.DuplicateData);
            }

            var user = registerDto.Adapt<ApplicationUser>();
            user.CreatedBy = DefaultValues.SystemUser;
            user.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var result = await _userManager.CreateAsync(user, registerDto.Password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new ArgumentException($"فشل التسجيل: {errors}");
                }

                var customer = new Customer { UserId = user.Id };
                await _unitOfWork.Customers.AddNewCustomer(customer);
                await _unitOfWork.SaveChangesAsync(); // Save to generate customer.Id

                await _userManager.AddToRoleAsync(user, Roles.Customer);

                // بعد إنشاء العميل
                var wallet = new Wallet
                {
                    CustomerId = customer.Id,
                    Balance = 0,
                    CreatedAt = DateTime.UtcNow
                };
                await _unitOfWork.Wallets.CreateAsync(wallet);
                await _unitOfWork.SaveChangesAsync();

                var authModel = await GenerateJwtTokenAsync(user, customer.Id);

                await _unitOfWork.CommitTransactionAsync();
                return authModel;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<AuthModel> LoginAsync(LoginRequestDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.PhoneNumber);
            if (user == null)
            {
                throw new KeyNotFoundException(ValidationMessages.UserNotFound);
            }

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                throw new UnauthorizedAccessException(ValidationMessages.InvalidCredentials);
            }

            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException(ValidationMessages.AccountInactive);
            }

            var customer = await _unitOfWork.Customers.GetCustomerByUserId(user.Id);
            if (customer == null)
            {
                throw new KeyNotFoundException("ملف العميل المرتبط بهذا الحساب غير موجود.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Any())
            {
                await _userManager.AddToRoleAsync(user, Roles.Customer);
            }

            var authModel = await GenerateJwtTokenAsync(user, customer.Id);

            // ✅ ==== أضف هذا الكود للتشخيص ====
            try
            {
                // نسجل رسالة قبل الحفظ مباشرة
                _logger.LogInformation("--> Attempting to save changes via UnitOfWork...");

                // نحفظ نتيجة الدالة في متغير
                var result = await _unitOfWork.SaveChangesAsync();

                // نسجل عدد السجلات التي تم حفظها
                _logger.LogInformation("--> UnitOfWork.SaveChangesAsync() completed. {Count} state entries written to the database.", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "--> FAILED to save changes via UnitOfWork.");
                throw; // إعادة رمي الخطأ لمعرفة ما هو
            }
            // ✅ ===============================

            return authModel;
        }
        public async Task<AuthModel> DashboardLoginAsync(LoginRequestDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.PhoneNumber);
            if (user == null)
            {
                throw new KeyNotFoundException(ValidationMessages.UserNotFound);
            }

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                throw new UnauthorizedAccessException(ValidationMessages.InvalidCredentials);
            }

            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException(ValidationMessages.AccountInactive);
            }

            var roles = await _userManager.GetRolesAsync(user);
            // التحقق من أن المستخدم لديه صلاحية وصول للداشبورد
            if (!roles.Contains(Roles.Admin) && !roles.Contains(Roles.Manager) && !roles.Contains(Roles.Employee))
            {
                throw new UnauthorizedAccessException(ValidationMessages.UnauthorizedAccess);
            }

            // لا نحتاج لجلب بيانات العميل هنا
            var customer = await _unitOfWork.Customers.GetCustomerByUserId(user.Id);
            int customerId = customer?.Id ?? 0; // قد لا يكون للموظف حساب عميل

            var authModel = await GenerateJwtTokenAsync(user, customerId);
            await _unitOfWork.SaveChangesAsync();
            return authModel;
        }

        public async Task<AuthModel> RefreshTokenAsync(RefreshTokenRequestDto refreshTokenDto)
        {
            var principal = GetPrincipalFromExpiredToken(refreshTokenDto.Token);
            if (principal?.FindFirstValue(Claims.UserId) is not { } userId)
            {
                throw new SecurityTokenException("الرمز المميز غير صالح.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new SecurityTokenException(ValidationMessages.UserNotFound);
            }

            var storedRefreshToken = await _unitOfWork.RefreshTokenRepo.GetRefreshTokenAsync(refreshTokenDto.RefreshToken, userId);
            if (storedRefreshToken == null || storedRefreshToken.IsRevoked || storedRefreshToken.ExpiryDate < DateTime.UtcNow)
            {
                throw new SecurityTokenException("رمز التحديث غير صالح أو منتهي الصلاحية.");
            }

            storedRefreshToken.IsRevoked = true;
            storedRefreshToken.LastModifiedAt = DateTime.UtcNow;
            storedRefreshToken.LastModifiedBy = userId;
            await _unitOfWork.RefreshTokenRepo.UpdateRefreshTokenAsync(storedRefreshToken);

            // Fetch customer ID again for the new token
            var customer = await _unitOfWork.Customers.GetCustomerByUserId(user.Id);
            if (customer == null) throw new InvalidOperationException("لا يمكن العثور على العميل المرتبط.");

            var newAuthModel = await GenerateJwtTokenAsync(user, customer.Id);

            await _unitOfWork.SaveChangesAsync(); // Commit the revocation and the new refresh token
            return newAuthModel;
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            var refreshToken = await _unitOfWork.RefreshTokenRepo.GetRefreshTokenByTokenAsync(token);
            if (refreshToken == null)
            {
                // Throwing an exception might be better to signal a 404 Not Found in the controller.
                // Returning false is also a valid strategy if you want to avoid exceptions for non-critical paths.
                throw new KeyNotFoundException("الرمز المميز المراد إلغاؤه غير موجود.");
            }

            refreshToken.IsRevoked = true;
            refreshToken.RevokedAt = DateTime.UtcNow;
            refreshToken.RevokedBy = _currentUserService.GetCurrentUserId();

            await _unitOfWork.RefreshTokenRepo.UpdateRefreshTokenAsync(refreshToken);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddToRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<bool> RemoveFromRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || !await _roleManager.RoleExistsAsync(roleName)) return false;

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<AuthModel?> GetUserInfoAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // Let the controller handle the null and return a 404
                return null;
            }

            var customer = await _unitOfWork.Customers.GetCustomerByUserId(user.Id);
            var roles = await _userManager.GetRolesAsync(user);

            return new AuthModel
            {
                UserId = user.Id,
                CustomerId = customer?.Id ?? 0,
                FullName = user.Name,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber!,
                Roles = roles.ToList(),
                IsAuthenticated = true
            };
        }

        // الكود المعدل في AuthService.cs
        private async Task<AuthModel> GenerateJwtTokenAsync(ApplicationUser user, int customerId)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
    {
        // استخدام الأنواع القياسية
        new Claim(ClaimTypes.NameIdentifier, user.Id), // <-- تغيير هنا
        new Claim(ClaimTypes.Name, user.Name), // <-- تغيير هنا
        new Claim(ClaimTypes.MobilePhone, user.PhoneNumber!), // <-- تغيير هنا
        new Claim("CustomerId", customerId.ToString()), // هذا claim مخصص وهو صحيح كما هو
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            // إضافة الأدوار (Roles) بالطريقة الصحيحة
            // يتم إضافة claim منفصل لكل دور
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role)); // <-- تغيير مهم هنا
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMonths(1),
                signingCredentials: creds
            );

            // ... باقي الكود يبقى كما هو
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            var refreshToken = new RefreshToken
            {
                Token = GenerateRefreshToken(),
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddMonths(1),
                CreatedBy = user.Id,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.RefreshTokenRepo.AddRefreshTokenAsync(refreshToken);

            return new AuthModel
            {
                Token = tokenString,
                RefreshToken = refreshToken.Token,
                TokenExpiry = token.ValidTo,
                RefreshTokenExpiry = refreshToken.ExpiryDate,
                UserId = user.Id,
                CustomerId = customerId,
                FullName = user.Name,
                PhoneNumber = user.PhoneNumber!,
                Roles = roles.ToList(),
                IsAuthenticated = true
            };
        }

        private string GenerateRefreshToken() => Guid.NewGuid().ToString("N");

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("الرمز المميز غير صالح.");
            }

            return principal;
        }
    }
}
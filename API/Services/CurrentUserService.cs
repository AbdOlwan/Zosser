using Shared.Interfaces;
using System.Security.Claims;

namespace API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CurrentUserService> _logger; // تغيير هنا

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor,
            ILogger<CurrentUserService> logger) // تغيير هنا
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public string? UserName => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public string? Email => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

        public string? PhoneNumber => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.MobilePhone)?.Value;

        public List<string> Roles => _httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role)?.Select(x => x.Value).ToList() ?? new List<string>();

        public string GetCurrentUserId()
        {
            return UserId ?? "System";
        }


        public int GetCustomerId()
        {
            try
            {
                var customerIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("CustomerId")?.Value;

                if (string.IsNullOrEmpty(customerIdClaim) || !int.TryParse(customerIdClaim, out int customerId))
                {
                    _logger.LogWarning("CustomerId claim not found or invalid");
                    throw new UnauthorizedAccessException("Customer identification missing");
                }

                return customerId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving customer ID");
                throw new ApplicationException("Failed to retrieve customer information", ex);
            }
        }

        public bool IsInRole(string role)
        {
            return Roles.Contains(role);
        }

        public bool IsInAnyRole(params string[] roles)
        {
            return roles.Any(role => Roles.Contains(role));
        }
    }
}

using Shared.DTOs.IdentityDtos;
using Shared.Models;

namespace BLL.Services.Interfaces.Identity
{
    public interface IAuthService
    {

        Task<AuthModel> RegisterAsync(RegisterRequestDto registerDto);
        Task<AuthModel> LoginAsync(LoginRequestDto loginDto);
        Task<AuthModel> DashboardLoginAsync(LoginRequestDto loginDto);
        Task<AuthModel> RefreshTokenAsync(RefreshTokenRequestDto refreshTokenDto);
        Task<bool> RevokeTokenAsync(string token);
        Task<bool> AddToRoleAsync(string userId, string roleName);
        Task<bool> RemoveFromRoleAsync(string userId, string roleName);
        Task<AuthModel?> GetUserInfoAsync(string userId);
    }
}

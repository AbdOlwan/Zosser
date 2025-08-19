using Shared.Constants;
using Shared.DTOs;
using Shared.DTOs.Dashboard;
using Shared.DTOs.IdentityDtos;
using Shared.Models;

namespace BLL.Services.Interfaces.Identity.Dashboard
{
    public interface IUserService
    {
        Task<ApiResponse<UserSummaryDto>> CreateStaffUserAsync(CreateStaffUserDto dto);
        Task<PagedResponse<UserSummaryDto>> GetAllUsersAsync(int pageNumber, int pageSize, string? roleFilter = null, string? searchTerm = null);
        Task<UserDetailsDto?> GetUserByIdAsync(string userId);
        Task<bool> UpdateUserStatusAsync(string userId, bool isActive);
        Task<bool> AddUserToRoleAsync(string userId, string roleName);
        Task<bool> RemoveUserFromRoleAsync(string userId, string roleName);
    }
}

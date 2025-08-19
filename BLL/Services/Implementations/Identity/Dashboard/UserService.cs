using BLL.Services.Interfaces.Identity.Dashboard;
using DAL.Entities.Identity;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.DTOs;
using Shared.DTOs.Dashboard;
using Shared.Interfaces;

namespace BLL.Services.Implementations.Identity.Dashboard
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICurrentUserService _currentUserService;

        public UserService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse<UserSummaryDto>> CreateStaffUserAsync(CreateStaffUserDto dto)
        {
            // 1. Check if user already exists
            var existingUser = await _userManager.FindByNameAsync(dto.PhoneNumber);
            if (existingUser != null)
            {
                return ApiResponse<UserSummaryDto>.CreateConflict(ApiMessages.DuplicateData);
            }

            // 2. Validate the role is a valid staff role
            if (dto.RoleName == Roles.Customer || !await _roleManager.RoleExistsAsync(dto.RoleName))
            {
                return ApiResponse<UserSummaryDto>.CreateBadRequest("صلاحية غير صالحة للموظفين.");
            }

            // 3. Create the user object
            var user = new ApplicationUser
            {
                UserName = dto.PhoneNumber,
                PhoneNumber = dto.PhoneNumber,
                Name = dto.FullName,
                Email = $"{dto.PhoneNumber}@system.local", // Or make it optional
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = _currentUserService.GetCurrentUserId() ?? DefaultValues.SystemUser // Assuming you have ICurrentUserService
            };

            // 4. Create user and add to role
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ApiResponse<UserSummaryDto>.CreateBadRequest($"فشل إنشاء المستخدم: {errors}");
            }

            await _userManager.AddToRoleAsync(user, dto.RoleName);

            // 5. Map to DTO and return success
            var userDto = user.Adapt<UserSummaryDto>();
            userDto.Roles = [dto.RoleName];

            return ApiResponse<UserSummaryDto>.CreateSuccess(userDto, "تم إنشاء حساب الموظف بنجاح.");
        }

        public async Task<PagedResponse<UserSummaryDto>> GetAllUsersAsync(int pageNumber, int pageSize, string? roleFilter = null, string? searchTerm = null)
        {
            var query = _userManager.Users;

            // Filtering
            if (!string.IsNullOrWhiteSpace(roleFilter))
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(roleFilter);
                var userIdsInRole = usersInRole.Select(u => u.Id).ToList();
                query = query.Where(u => userIdsInRole.Contains(u.Id));
            }
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(u => u.Name.Contains(searchTerm) || u.PhoneNumber!.Contains(searchTerm) || u.Email!.Contains(searchTerm));
            }

            // Pagination
            var totalCount = await query.CountAsync();
            var users = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            // Mapping to DTO
            var userDtos = new List<UserSummaryDto>();
            foreach (var user in users)
            {
                var dto = user.Adapt<UserSummaryDto>();
                dto.Roles = (await _userManager.GetRolesAsync(user)).ToList();
                userDtos.Add(dto);
            }

            return new PagedResponse<UserSummaryDto>
            {
                Items = userDtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }

        public async Task<bool> UpdateUserStatusAsync(string userId, bool isActive)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.IsActive = isActive;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<UserDetailsDto?> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return null;
            var userDto = user.Adapt<UserDetailsDto>();
            userDto.Roles = (await _userManager.GetRolesAsync(user)).ToList();
            return userDto;
        }
        public async Task<bool> AddUserToRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                return false; // Role does not exist
            }
            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }
        public async Task<bool> RemoveUserFromRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                return false; // Role does not exist
            }
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result.Succeeded;

        }
    }
}
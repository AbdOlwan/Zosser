using BLL.Services.Interfaces.Identity.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Shared.DTOs;
using Shared.DTOs.Dashboard;

namespace API.Controllers.Dashboard
{
    [Route("api/v1/dashboard/users")]
    [ApiController]
   // [Authorize(Policy = "RequireManagerAccess")] // يتطلب صلاحية مدير أو أدمن كحد أدنى
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        /// <summary>
        /// إنشاء مستخدم جديد (موظف، مدير، مشرف)
        /// </summary>
        /// <remarks>
        /// هذا الـ Endpoint مخصص للـ Admin فقط لإنشاء حسابات للموظفين.
        /// </remarks>
        [HttpPost]
       // [Authorize(Policy = Policies.RequireAdminRole)] // **مهم جداً: فقط الأدمن يمكنه الإنشاء**
        [ProducesResponseType(typeof(ApiResponse<UserSummaryDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateStaffUser([FromBody] CreateStaffUserDto dto)
        {
            if (!ModelState.IsValid)
                return ApiResponse<UserDetailsDto>
                      .CreateBadRequest(ModelState)
                                                    .ToActionResult();
            var response = await _userService.CreateStaffUserAsync(dto);

            return response.StatusCode switch
            {
                StatusCodes.Status200OK => CreatedAtAction(nameof(GetUserById), new { userId = response.Data!.UserId }, response),
                StatusCodes.Status400BadRequest => BadRequest(response),
                StatusCodes.Status409Conflict => Conflict(response),
                _ => StatusCode(StatusCodes.Status500InternalServerError, response)
            };
        }

        /// <summary>
        /// جلب قائمة المستخدمين مع فلترة وترقيم الصفحات
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<UserSummaryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? role = null, [FromQuery] string? search = null)
        {
            var users = await _userService.GetAllUsersAsync(pageNumber, pageSize, role, search);
            return Ok(ApiResponse<PagedResponse<UserSummaryDto>>.CreateSuccess(users, "تم جلب المستخدمين بنجاح."));
        }

        /// <summary>
        /// جلب تفاصيل مستخدم معين
        /// </summary>
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(ApiResponse<UserDetailsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound(ApiResponse.CreateNotFound($"المستخدم رقم {userId} غير موجود."));

            return Ok(ApiResponse<UserDetailsDto>.CreateSuccess(user, "تم جلب بيانات المستخدم بنجاح."));
        }

        /// <summary>
        /// تحديث حالة المستخدم (تفعيل/تعطيل)
        /// </summary>
        [HttpPut("{userId}/status")]
        [Authorize(Policy = Policies.RequireAdminRole)] // فقط الأدمن يستطيع تغيير الحالة
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserStatus(string userId, [FromBody] UpdateUserStatusDto dto)
        {
            var result = await _userService.UpdateUserStatusAsync(userId, dto.IsActive);
            if (!result)
                return BadRequest(ApiResponse.CreateBadRequest("فشل تحديث حالة المستخدم."));

            return Ok(ApiResponse.CreateSuccess("تم تحديث حالة المستخدم بنجاح."));
        }

        /// <summary>
        /// إضافة دور (Role) لمستخدم
        /// </summary>
        [HttpPost("{userId}/roles")]
        [Authorize(Policy = Policies.RequireAdminRole)] // فقط الأدمن يضيف أدوار
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddRoleToUser(string userId, [FromBody] ManageUserRolesDto dto)
        {
            var result = await _userService.AddUserToRoleAsync(userId, dto.RoleName);
            if (!result)
                return BadRequest(ApiResponse.CreateBadRequest($"فشل إضافة الدور '{dto.RoleName}' للمستخدم."));

            return Ok(ApiResponse.CreateSuccess("تم إضافة الدور بنجاح."));
        }

        /// <summary>
        /// حذف دور (Role) من مستخدم
        /// </summary>
        [HttpDelete("{userId}/roles")]
        [Authorize(Policy = Policies.RequireAdminRole)] // فقط الأدمن يحذف أدوار
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveRoleFromUser(string userId, [FromBody] ManageUserRolesDto dto)
        {
            var result = await _userService.RemoveUserFromRoleAsync(userId, dto.RoleName);
            if (!result)
                return BadRequest(ApiResponse.CreateBadRequest($"فشل حذف الدور '{dto.RoleName}' من المستخدم."));

            return Ok(ApiResponse.CreateSuccess("تم حذف الدور بنجاح."));
        }
    }
}

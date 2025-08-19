using BLL.Services.Interfaces.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shared.DTOs;
using Shared.DTOs.IdentityDtos;
using Shared.Models;

namespace API.Controllers.Identity
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        private string GetModelErrors()
        {
            return string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        }



        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<AuthModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(
                    ApiResponse.CreateBadRequest(GetModelErrors()).WithRequestId(HttpContext.TraceIdentifier)
                );
            try
            {
                var result = await _authService.RegisterAsync(dto);
                var response = ApiResponse<AuthModel>.CreateSuccess(result, "تم التسجيل بنجاح.");
                return Ok(response.WithRequestId(HttpContext.TraceIdentifier));
            }
            catch (ArgumentException ex) // For validation-like errors from the service
            {
                _logger.LogWarning(ex, "بيانات غير صحيحة للتسجيل: {Message}", ex.Message);
                return BadRequest(
                    ApiResponse.CreateBadRequest(ex.Message).WithRequestId(HttpContext.TraceIdentifier)
                );
            }
            catch (InvalidOperationException ex) // For conflicts like duplicate user
            {
                _logger.LogWarning(ex, "المستخدم موجود بالفعل: {Message}", ex.Message);
                return Conflict(
                    ApiResponse.CreateConflict(ex.Message).WithRequestId(HttpContext.TraceIdentifier)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "حدث خطأ غير متوقع أثناء التسجيل.");
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ApiResponse.CreateInternalServerError("حدث خطأ في الخادم أثناء معالجة طلبك.").WithRequestId(HttpContext.TraceIdentifier)
                );
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<AuthModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(
                    ApiResponse.CreateBadRequest(GetModelErrors()).WithRequestId(HttpContext.TraceIdentifier)
                );

            try
            {
                var result = await _authService.LoginAsync(dto);
                var response = ApiResponse<AuthModel>.CreateSuccess(result, "تم تسجيل الدخول بنجاح.");
                return Ok(response.WithRequestId(HttpContext.TraceIdentifier));
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "محاولة تسجيل دخول غير مصرح بها.");
                return Unauthorized(
                    ApiResponse.CreateUnauthorized(ex.Message).WithRequestId(HttpContext.TraceIdentifier)
                );
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "المستخدم غير موجود لمحاولة تسجيل الدخول.");
                return NotFound(
                    ApiResponse.CreateNotFound(ex.Message).WithRequestId(HttpContext.TraceIdentifier)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "حدث خطأ غير متوقع أثناء تسجيل الدخول.");
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ApiResponse.CreateInternalServerError("حدث خطأ في الخادم أثناء معالجة طلبك.").WithRequestId(HttpContext.TraceIdentifier)
                );
            }
        }


        /// <summary>
        /// تسجيل دخول الموظفين (للداشبورد)
        /// </summary>
        [HttpPost("dashboard/login")]
        [ProducesResponseType(typeof(ApiResponse<AuthModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DashboardLogin([FromBody] LoginRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(
                    ApiResponse.CreateBadRequest(GetModelErrors()).WithRequestId(HttpContext.TraceIdentifier)
                );

            try
            {
                var result = await _authService.DashboardLoginAsync(dto);
                var response = ApiResponse<AuthModel>.CreateSuccess(result, "تم تسجيل الدخول بنجاح.");
                return Ok(response.WithRequestId(HttpContext.TraceIdentifier));
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "محاولة تسجيل دخول غير مصرح بها للداشبورد.");
                return Unauthorized(
                    ApiResponse.CreateUnauthorized(ex.Message).WithRequestId(HttpContext.TraceIdentifier)
                );
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "المستخدم غير موجود لمحاولة تسجيل الدخول.");
                return NotFound(
                    ApiResponse.CreateNotFound(ex.Message).WithRequestId(HttpContext.TraceIdentifier)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "حدث خطأ غير متوقع أثناء تسجيل دخول الداشبورد.");
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ApiResponse.CreateInternalServerError("حدث خطأ في الخادم أثناء معالجة طلبك.").WithRequestId(HttpContext.TraceIdentifier)
                );
            }
        }


        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(ApiResponse<AuthModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(
                    ApiResponse.CreateBadRequest(GetModelErrors()).WithRequestId(HttpContext.TraceIdentifier)
                );

            try
            {
                var result = await _authService.RefreshTokenAsync(dto);
                var response = ApiResponse<AuthModel>.CreateSuccess(result, "تم تجديد الرمز المميز بنجاح.");
                return Ok(response.WithRequestId(HttpContext.TraceIdentifier));
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogWarning(ex, "رمز مميز أو رمز تحديث غير صالح.");
                return Unauthorized(
                    ApiResponse.CreateUnauthorized(ex.Message).WithRequestId(HttpContext.TraceIdentifier)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "حدث خطأ غير متوقع أثناء تجديد الرمز.");
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ApiResponse.CreateInternalServerError("حدث خطأ في الخادم أثناء معالجة طلبك.").WithRequestId(HttpContext.TraceIdentifier)
                );
            }
        }

        [HttpPost("revoke-token")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequestDto dto) // Changed to DTO
        {
            var token = dto.RefreshToken; // Use the refresh token from the DTO
            if (string.IsNullOrWhiteSpace(token))
                return BadRequest(
                    ApiResponse.CreateBadRequest("رمز التحديث مطلوب.").WithRequestId(HttpContext.TraceIdentifier)
                );

            try
            {
                await _authService.RevokeTokenAsync(token);
                return Ok(
                    ApiResponse.CreateSuccess("تم إلغاء صلاحية الرمز بنجاح.").WithRequestId(HttpContext.TraceIdentifier)
                );
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "محاولة إلغاء رمز غير موجود.");
                return NotFound(ApiResponse.CreateNotFound(ex.Message).WithRequestId(HttpContext.TraceIdentifier));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "حدث خطأ غير متوقع أثناء إلغاء الرمز.");
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ApiResponse.CreateInternalServerError("حدث خطأ في الخادم أثناء معالجة طلبك.").WithRequestId(HttpContext.TraceIdentifier)
                );
            }
        }

        [HttpGet("user-info/{userId}")]
        [ProducesResponseType(typeof(ApiResponse<AuthModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserInfo([FromRoute] string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest(
                    ApiResponse.CreateBadRequest("معرف المستخدم مطلوب.").WithRequestId(HttpContext.TraceIdentifier)
                );

            try
            {
                var result = await _authService.GetUserInfoAsync(userId);
                if (result == null)
                {
                    return NotFound(
                        ApiResponse.CreateNotFound($"المستخدم رقم {userId} غير موجود.").WithRequestId(HttpContext.TraceIdentifier)
                    );
                }

                return Ok(
                    ApiResponse<AuthModel>.CreateSuccess(result, "تم جلب بيانات المستخدم بنجاح.").WithRequestId(HttpContext.TraceIdentifier)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "حدث خطأ أثناء جلب بيانات المستخدم {UserId}", userId);
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ApiResponse.CreateInternalServerError("حدث خطأ في الخادم.").WithRequestId(HttpContext.TraceIdentifier)
                );
            }
        }
    }
}
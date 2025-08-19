using BLL.Services.Interfaces.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Shared.DTOs;
using Shared.DTOs.CustomerDtos;

namespace API.Controllers.Customers
{
    //[Authorize(Roles = Roles.Admin)]
    [Route("api/v1/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(
            ICustomerService customerService,
            ILogger<CustomersController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = Policies.RequireAdminRole)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var customers = await _customerService.GetAllCustomersAsync();
                return Ok(ApiResponse<IEnumerable<CustomerSummaryDto>>.CreateSuccess(customers, "تم جلب العملاء بنجاح")
                    .WithRequestId(HttpContext.TraceIdentifier));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "حدث خطأ أثناء جلب العملاء");
                return StatusCode(500, ApiResponse<IEnumerable<CustomerSummaryDto>>.CreateInternalServerError("حدث خطأ في الخادم")
                    .WithRequestId(HttpContext.TraceIdentifier));
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var customer = await _customerService.GetCustomerByIdAsync(id);
                return Ok(ApiResponse<CustomerDto>.CreateSuccess(customer, "تم جلب العميل بنجاح")
                    .WithRequestId(HttpContext.TraceIdentifier));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "العميل غير موجود - ID: {CustomerId}", id);
                return NotFound(ApiResponse<CustomerDto>.CreateNotFound($"العميل رقم {id} غير موجود")
                    .WithRequestId(HttpContext.TraceIdentifier));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "حدث خطأ أثناء جلب العميل - ID: {CustomerId}", id);
                return StatusCode(500, ApiResponse<CustomerDto>.CreateInternalServerError("حدث خطأ في الخادم")
                    .WithRequestId(HttpContext.TraceIdentifier));
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> Create(CreateCustomerDto dto)
        //{
        //    try
        //    {
        //        // التحقق من صحة البيانات
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ApiResponse<CustomerDto>.CreateBadRequest(ModelState.ToString()!)
        //                .WithRequestId(HttpContext.TraceIdentifier));
        //        }

        //        var newCustomer = await _customerService.CreateCustomerAsync(dto);
        //        return CreatedAtAction(
        //            nameof(GetById),
        //            new { id = newCustomer.Id },
        //            ApiResponse<CustomerDto>.CreateCreated(newCustomer, "تم إنشاء العميل بنجاح")
        //                .WithRequestId(HttpContext.TraceIdentifier)
        //        );
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        _logger.LogWarning(ex, "بيانات غير صحيحة لإنشاء العميل");
        //        return BadRequest(ApiResponse<CustomerDto>.CreateBadRequest(ex.Message)
        //            .WithRequestId(HttpContext.TraceIdentifier));
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        _logger.LogWarning(ex, "عملية غير صحيحة لإنشاء العميل");
        //        return Conflict(ApiResponse<CustomerDto>.CreateConflict(ex.Message)
        //            .WithRequestId(HttpContext.TraceIdentifier));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "حدث خطأ أثناء إنشاء عميل");
        //        return StatusCode(500, ApiResponse<CustomerDto>.CreateInternalServerError("حدث خطأ في الخادم")
        //            .WithRequestId(HttpContext.TraceIdentifier));
        //    }
        //}

        [HttpPut]
        public async Task<IActionResult> Update(UpdateCustomerDto dto)
        {
            try
            {
                // التحقق من صحة البيانات
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<CustomerDto>.CreateBadRequest(ModelState.Values.ToString()!)
                        .WithRequestId(HttpContext.TraceIdentifier));
                }

                var updatedCustomer = await _customerService.UpdateCustomerAsync(dto);
                return Ok(ApiResponse<CustomerDto>.CreateSuccess(updatedCustomer, "تم تحديث العميل بنجاح")
                    .WithRequestId(HttpContext.TraceIdentifier));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "العميل غير موجود للتحديث - ID: {CustomerId}", dto.Id);
                return NotFound(ApiResponse<CustomerDto>.CreateNotFound($"العميل رقم {dto.Id} غير موجود")
                    .WithRequestId(HttpContext.TraceIdentifier));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "بيانات غير صحيحة لتحديث العميل");
                return BadRequest(ApiResponse<CustomerDto>.CreateBadRequest(ex.Message)
                    .WithRequestId(HttpContext.TraceIdentifier));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "عملية غير صحيحة لتحديث العميل");
                return Conflict(ApiResponse<CustomerDto>.CreateConflict(ex.Message)
                    .WithRequestId(HttpContext.TraceIdentifier));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "حدث خطأ أثناء تحديث العميل");
                return StatusCode(500, ApiResponse<CustomerDto>.CreateInternalServerError("حدث خطأ في الخادم")
                    .WithRequestId(HttpContext.TraceIdentifier));
            }
        }

        [HttpPatch("toggle-status")]
        [Authorize(Policy = Policies.RequireAdminRole)]
        public async Task<IActionResult> ToggleStatus(ToggleCustomerStatusDto dto)
        {
            try
            {
                // التحقق من صحة البيانات
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse.CreateBadRequest()
                        .WithRequestId(HttpContext.TraceIdentifier));
                }

                await _customerService.ToggleCustomerStatusAsync(dto);
                return Ok(ApiResponse.CreateSuccess("تم تغيير حالة العميل بنجاح")
                    .WithRequestId(HttpContext.TraceIdentifier));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "العميل غير موجود لتغيير الحالة - ID: {CustomerId}", dto.Id);
                return NotFound(ApiResponse.CreateNotFound($"العميل رقم {dto.Id} غير موجود")
                    .WithRequestId(HttpContext.TraceIdentifier));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "بيانات غير صحيحة لتغيير حالة العميل");
                return BadRequest(ApiResponse.CreateBadRequest(ex.Message)
                    .WithRequestId(HttpContext.TraceIdentifier));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "حدث خطأ أثناء تغيير حالة العميل");
                return StatusCode(500, ApiResponse.CreateInternalServerError("حدث خطأ في الخادم")
                    .WithRequestId(HttpContext.TraceIdentifier));
            }
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            try
            {
                // التحقق من صحة البيانات
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse.CreateBadRequest("Bad Request")
                        .WithRequestId(HttpContext.TraceIdentifier));
                }

                await _customerService.ChangePasswordAsync(dto);
                return Ok(ApiResponse.CreateSuccess("تم تغيير كلمة المرور بنجاح")
                    .WithRequestId(HttpContext.TraceIdentifier));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "العميل غير موجود لتغيير كلمة المرور - ID: {CustomerId}", dto.UserId);
                return NotFound(ApiResponse.CreateNotFound($"العميل رقم {dto.UserId} غير موجود")
                    .WithRequestId(HttpContext.TraceIdentifier));
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "محاولة غير مصرح بها لتغيير كلمة المرور");
                return Unauthorized(ApiResponse.CreateUnauthorized("كلمة المرور الحالية غير صحيحة")
                    .WithRequestId(HttpContext.TraceIdentifier));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "بيانات غير صحيحة لتغيير كلمة المرور");
                return BadRequest(ApiResponse.CreateBadRequest(ex.Message)
                    .WithRequestId(HttpContext.TraceIdentifier));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "حدث خطأ أثناء تغيير كلمة المرور");
                return StatusCode(500, ApiResponse.CreateInternalServerError("حدث خطأ في الخادم")
                    .WithRequestId(HttpContext.TraceIdentifier));
            }
        }

        //[HttpDelete("{id:int}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    try
        //    {
        //        await _customerService.DeleteCustomerAsync(id);
        //        return Ok(ApiResponse.CreateSuccess("تم حذف العميل بنجاح")
        //            .WithRequestId(HttpContext.TraceIdentifier));
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        _logger.LogWarning(ex, "العميل غير موجود للحذف - ID: {CustomerId}", id);
        //        return NotFound(ApiResponse.CreateNotFound($"العميل رقم {id} غير موجود")
        //            .WithRequestId(HttpContext.TraceIdentifier));
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        _logger.LogWarning(ex, "لا يمكن حذف العميل - ID: {CustomerId}", id);
        //        return Conflict(ApiResponse.CreateConflict(ex.Message)
        //            .WithRequestId(HttpContext.TraceIdentifier));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "حدث خطأ أثناء حذف العميل");
        //        return StatusCode(500, ApiResponse.CreateInternalServerError("حدث خطأ في الخادم")
        //            .WithRequestId(HttpContext.TraceIdentifier));
        //    }
        //}
    }
}
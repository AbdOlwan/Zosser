using BLL.Services.Interfaces.Identity;
using DAL.Entities.Identity;
using DAL.Repositories.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shared.Constants;
using Shared.DTOs;
using Shared.DTOs.CustomerDtos;
using Shared.Interfaces;

namespace BLL.Services.Implementations.Identity
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public CustomerService(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task<CustomerDto> GetCustomerByIdAsync(int id)
        {
            var customer = await _unitOfWork.Customers.GetCustomerById(id);
            if (customer == null) throw new KeyNotFoundException(ApiMessages.NotFound);

            return MapToCustomerDto(customer);
        }

        public async Task<CustomerDto> UpdateCustomerAsync(UpdateCustomerDto dto)
        {
            // 1. ابحث عن كيان العميل من قاعدة البيانات
            var customer = await _unitOfWork.Customers.GetCustomerById(dto.Id);
            if (customer == null) throw new KeyNotFoundException("العميل غير موجود.");

            // 2. ابحث عن كيان المستخدم المرتبط به
            var user = await _userManager.FindByIdAsync(customer.UserId);
            if (user == null) throw new KeyNotFoundException("المستخدم المرتبط بالعميل غير موجود.");

            // 3. قم بتحديث خصائص المستخدم بشكل يدوي وآمن
            //    هذا يمنع تعديل المفتاح الأساسي (user.Id)
            user.Name = dto.FullName;
            user.Email = dto.Email;
            user.Gender = dto.Gender;
            user.DateOfBirth = dto.DateOfBirth;
            user.LastModifiedAt = DateTime.UtcNow;
            user.LastModifiedBy = _currentUserService.GetCurrentUserId();


            // 4. قم بتحديث المستخدم باستخدام UserManager
            var identityResult = await _userManager.UpdateAsync(user);
            if (!identityResult.Succeeded)
            {
                // في حالة فشل تحديث المستخدم، قم بإظهار الأخطاء
                var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"فشل تحديث بيانات المستخدم: {errors}");
            }

            // 5. تحديث تاريخ التعديل في سجل العميل أيضاً (اختياري لكنه ممارسة جيدة)
            customer.LastModifiedAt = DateTime.UtcNow;
            customer.LastModifiedBy = user.LastModifiedBy;
            await _unitOfWork.Customers.UpdateCustomer(customer); // استدعاء Repo لتحديث العميل

            // 6. حفظ جميع التغييرات في قاعدة البيانات
            await _unitOfWork.SaveChangesAsync();

            // 7. إرجاع البيانات المحدثة
            return MapToCustomerDto(customer);
        }

        public async Task<bool> ToggleCustomerStatusAsync(ToggleCustomerStatusDto dto)
        {
            var customer = await _unitOfWork.Customers.GetCustomerById(dto.Id);
            if (customer == null) throw new KeyNotFoundException(ApiMessages.NotFound);

            var user = await _userManager.FindByIdAsync(customer.UserId);
            if (user == null) throw new KeyNotFoundException(ApiMessages.NotFound);

            user.IsActive = dto.IsActive;
            user.LastModifiedAt = DateTime.UtcNow;
            user.LastModifiedBy = _currentUserService.GetCurrentUserId();

            if( await _userManager.UpdateAsync(user) == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null) throw new KeyNotFoundException(ApiMessages.NotFound);

            return (await _userManager.ChangePasswordAsync(
                user,
                dto.CurrentPassword,
                dto.NewPassword
            )).Succeeded;
        }

        public async Task<IEnumerable<CustomerSummaryDto>> GetAllCustomersAsync()
        {
            var customers = await _unitOfWork.Customers.GetAllCustomers();
            return customers.Adapt<List<CustomerSummaryDto>>();
        }

        private CustomerDto MapToCustomerDto(Customer customer)
        {
            var user = _userManager.FindByIdAsync(customer.UserId).Result;
            var dto = customer.Adapt<CustomerDto>();
            dto.FullName = user!.Name;
            dto.Email = user?.Email;
            dto.PhoneNumber = user!.PhoneNumber!;
            dto.Gender = user?.Gender;
            dto.DateOfBirth = user?.DateOfBirth;
            dto.IsActive = user?.IsActive ?? false;
            dto.Roles = _userManager.GetRolesAsync(user!).Result.ToList();
            return dto;
        }
    }
}

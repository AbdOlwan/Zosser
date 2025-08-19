using DAL.Context;
using DAL.Entities.Identity;
using DAL.Repositories.Interfaces.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implementations.Identity
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CustomerRepo> _logger;

        public CustomerRepo(ApplicationDbContext context, ILogger<CustomerRepo> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            try
            {
                _logger.LogInformation("جاري استرجاع جميع العملاء");

                return await _context.Customers
                    .Include(c => c.User)
                    .OrderBy(c => c.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في استرجاع جميع العملاء");
                throw;
            }
        }

        public async Task<Customer?> GetCustomerById(int id)
        {
            try
            {
                _logger.LogInformation("جاري استرجاع العميل بالمعرف: {CustomerId}", id);

                return await _context.Customers
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في استرجاع العميل بالمعرف: {CustomerId}", id);
                throw;
            }
        }

        public async Task<Customer?> GetCustomerByUserId(string userId)
        {
            try
            {
                _logger.LogInformation("جاري استرجاع العميل بمعرف المستخدم: {UserId}", userId);

                return await _context.Customers
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.UserId == userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في استرجاع العميل بمعرف المستخدم: {UserId}", userId);
                throw;
            }
        }

        public async Task<Customer> AddNewCustomer(Customer customer)
        {
            try
            {
                _logger.LogInformation("جاري إضافة عميل جديد لمعرف المستخدم: {UserId}", customer.UserId);

                // التحقق من وجود المستخدم
                var userExists = await _context.Users.AnyAsync(u => u.Id == customer.UserId);
                if (!userExists)
                {
                    throw new InvalidOperationException($"المستخدم بالمعرف {customer.UserId} غير موجود");
                }

                // التحقق من عدم وجود عميل مرتبط بنفس المستخدم
                var existingCustomer = await _context.Customers
                    .AnyAsync(c => c.UserId == customer.UserId);
                if (existingCustomer)
                {
                    throw new InvalidOperationException($"يوجد عميل مرتبط بالمستخدم {customer.UserId} مسبقاً");
                }

                await _context.Customers.AddAsync(customer);
                await _context.SaveChangesAsync();

                _logger.LogInformation("تم إضافة العميل بنجاح بالمعرف: {CustomerId}", customer.Id);

                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في إضافة العميل لمعرف المستخدم: {UserId}", customer.UserId);
                throw;
            }
        }

        public async Task<bool> UpdateCustomer(Customer customer)
        {
            try
            {
                _logger.LogInformation("جاري تحديث العميل بالمعرف: {CustomerId}", customer.Id);

                // التحقق من وجود العميل
                var existingCustomer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Id == customer.Id);

                if (existingCustomer == null)
                {
                    _logger.LogWarning("العميل بالمعرف {CustomerId} غير موجود", customer.Id);
                    return false;
                }

                // تحديث الخصائص
                existingCustomer.UserId = customer.UserId;
                existingCustomer.LastModifiedAt = DateTime.UtcNow;
                existingCustomer.LastModifiedBy = customer.LastModifiedBy;

                _context.Customers.Update(existingCustomer);
                var result = await _context.SaveChangesAsync();

                _logger.LogInformation("تم تحديث العميل بنجاح بالمعرف: {CustomerId}", customer.Id);

                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في تحديث العميل بالمعرف: {CustomerId}", customer.Id);
                throw;
            }
        }

        public async Task<bool> DeleteCustomerById(int id)
        {
            try
            {
                _logger.LogInformation("جاري حذف العميل بالمعرف: {CustomerId}", id);

                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);

                if (customer == null)
                {
                    _logger.LogWarning("العميل بالمعرف {CustomerId} غير موجود", id);
                    return false;
                }

                _context.Customers.Remove(customer);
                var result = await _context.SaveChangesAsync();

                _logger.LogInformation("تم حذف العميل بنجاح بالمعرف: {CustomerId}", id);

                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في حذف العميل بالمعرف: {CustomerId}", id);
                throw;
            }
        }

        public async Task<Customer?> GetCustomerWithDetailsAsync(int customerId)
        {
            try
            {
                _logger.LogInformation("جاري استرجاع العميل مع التفاصيل بالمعرف: {CustomerId}", customerId);

                return await _context.Customers
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == customerId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في استرجاع العميل مع التفاصيل بالمعرف: {CustomerId}", customerId);
                throw;
            }
        }

        public async Task<bool> CustomerExistsAsync(int customerId)
        {
            try
            {
                _logger.LogInformation("جاري التحقق من وجود العميل بالمعرف: {CustomerId}", customerId);

                return await _context.Customers.AnyAsync(c => c.Id == customerId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في التحقق من وجود العميل بالمعرف: {CustomerId}", customerId);
                throw;
            }
        }

        public async Task<bool> CustomerExistsByUserIdAsync(string userId)
        {
            try
            {
                _logger.LogInformation("جاري التحقق من وجود العميل بمعرف المستخدم: {UserId}", userId);

                return await _context.Customers.AnyAsync(c => c.UserId == userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في التحقق من وجود العميل بمعرف المستخدم: {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> CustomerExistsByEmailAsync(string email)
        {
            try
            {
                _logger.LogInformation("جاري التحقق من وجود العميل بالبريد الإلكتروني: {Email}", email);

                return await _context.Customers
                    .Include(c => c.User)
                    .AnyAsync(c => c.User.Email == email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في التحقق من وجود العميل بالبريد الإلكتروني: {Email}", email);
                throw;
            }
        }

        public async Task<bool> CustomerExistsByPhoneAsync(string phoneNumber)
        {
            try
            {
                _logger.LogInformation("جاري التحقق من وجود العميل برقم الهاتف: {PhoneNumber}", phoneNumber);

                return await _context.Customers
                    .Include(c => c.User)
                    .AnyAsync(c => c.User.PhoneNumber == phoneNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في التحقق من وجود العميل برقم الهاتف: {PhoneNumber}", phoneNumber);
                throw;
            }
        }
    }
}

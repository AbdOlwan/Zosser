namespace Shared.Constants
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";
        public const string Employee = "Employee";
        public const string Manager = "Manager";
    }

    public static class Policies
    {
        public const string RequireAdminRole = "RequireAdminRole";
        public const string RequireCustomerRole = "RequireCustomerRole";
        public const string RequireEmployeeRole = "RequireEmployeeRole";
        public const string RequireManagerRole = "RequireManagerRole";
    }

    public static class Gender
    {
        public const string Male = "ذكر";
        public const string Female = "أنثى";
    }

    public static class Claims
    {
        public const string UserId = "UserId";
        public const string Name = "Name";
        public const string Email = "Email";
        public const string PhoneNumber = "PhoneNumber";
        public const string Role = "Role";
    }

    public static class ValidationMessages
    {
        public const string Required = "هذا الحقل مطلوب";
        public const string InvalidFormat = "صيغة البيانات غير صحيحة";
        public const string InvalidPhoneNumber = "رقم الهاتف غير صحيح";
        public const string InvalidEmail = "البريد الإلكتروني غير صحيح";
        public const string PasswordTooShort = "كلمة المرور قصيرة جداً";
        public const string PasswordsDoNotMatch = "كلمات المرور غير متطابقة";
        public const string UserNotFound = "المستخدم غير موجود";
        public const string InvalidCredentials = "بيانات الدخول غير صحيحة";
        public const string AccountInactive = "الحساب غير مفعل";
        public const string UnauthorizedAccess = "غير مصرح لك بالوصول";
    }

    public static class ApiMessages
    {
        public const string Success = "تم بنجاح";
        public const string Created = "تم الإنشاء بنجاح";
        public const string Updated = "تم التحديث بنجاح";
        public const string Deleted = "تم الحذف بنجاح";
        public const string NotFound = "غير موجود";
        public const string BadRequest = "طلب غير صحيح";
        public const string InternalServerError = "حدث خطأ في الخادم";
        public const string ValidationError = "خطأ في التحقق من البيانات";
        public const string DuplicateData = "البيانات مكررة";
    }

    public static class DefaultValues
    {
        public const string SystemUser = "System";
        public const int DefaultPageSize = 10;
        public const int MaxPageSize = 100;
        public const int TokenExpirationMinutes = 30;
        public const int RefreshTokenExpirationDays = 7;
    }

    public class PagedResponse<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}

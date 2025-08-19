namespace Shared.Interfaces
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? UserName { get; }
        string? Email { get; }
        string? PhoneNumber { get; }
        List<string> Roles { get; }
        bool IsAuthenticated { get; }

        /// <summary>
        /// الحصول على معرف المستخدم الحالي أو "System" إذا لم يكن مسجل دخول
        /// </summary>
        /// <returns>معرف المستخدم</returns>
        string GetCurrentUserId();

        /// <summary>
        /// الحصول على معرف العميل الحالي
        /// </summary>
        /// <returns>معرف العميل</returns>
        int GetCustomerId();

        /// <summary>
        /// التحقق من وجود المستخدم في دور معين
        /// </summary>
        /// <param name="role">اسم الدور</param>
        /// <returns>نتيجة التحقق</returns>
        bool IsInRole(string role);

        /// <summary>
        /// التحقق من وجود المستخدم في أي من الأدوار المحددة
        /// </summary>
        /// <param name="roles">قائمة الأدوار</param>
        /// <returns>نتيجة التحقق</returns>
        bool IsInAnyRole(params string[] roles);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration; // تم إضافة هذا الـ using

namespace DAL.Context
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // تحديد المسار الأساسي لملف appsettings.json
            // يتم الرجوع مجلد واحد للوصول إلى مجلد الحل، ثم الدخول إلى مجلد API
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "API");

            // بناء كائن Configuration من ملف appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(basePath) // تحديد المسار الأساسي لملفات الإعدادات
                .AddJsonFile("appsettings.json") // إضافة ملف appsettings.json
                .Build(); // بناء كائن الإعدادات

            // استخراج سلسلة الاتصال
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Could not find connection string 'DefaultConnection'");
            }

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            // إرجاع نسخة من ApplicationDbContext باستخدام الخيارات التي تم تكوينها
            // لاحظ أننا لا نوفر ICurrentUserService هنا لأنه غير متاح في وقت التصميم
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
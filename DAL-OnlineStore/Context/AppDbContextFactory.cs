using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace DAL_OnlineStore.Context
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "API-OnlineStore");
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);

            // إنشاء مثيل من OperationalStoreOptions (يمكنك تعديل القيم إذا لزم الأمر)
            var operationalStoreOptions = new OperationalStoreOptions
            {
                // هنا يمكنك تعيين خصائص OperationalStoreOptions إذا كنت بحاجة إلى تخصيصها
                // DefaultSchema = "YourSchema",
                // ...
            };

            // تغليف OperationalStoreOptions في IOptions
            var operationalStoreOptionsWrapper = Options.Create(operationalStoreOptions);

            // تمرير كلا المعاملين إلى مُنشئ AppDbContext
            return new AppDbContext(optionsBuilder.Options, operationalStoreOptionsWrapper);
        }
    }
}
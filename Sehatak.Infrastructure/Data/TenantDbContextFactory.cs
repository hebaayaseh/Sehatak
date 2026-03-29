using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace Sehatak.Infrastructure.Data
{
    public class TenantDbContextFactory
    {
        private readonly IConfiguration _config;

        public TenantDbContextFactory(IConfiguration config)
        {
            _config = config;
        }

        // بتبني TenantDbContext لمركز معين بناءً على الـ CenterId
        public TenantDbContext CreateForCenter(int centerId)
        {
            // بنبني اسم الداتا بيس
            var dbName = $"sehatak_center_{centerId}";

            // بنجيب الـ Template من الـ appsettings
            var template = _config.GetConnectionString("TenantDbTemplate");

            // بنضيف اسم الداتا بيس للـ Connection String
            var connectionString = $"{template}Database={dbName};";

            // بنبني الـ Options
            var optionsBuilder = new DbContextOptionsBuilder<TenantDbContext>();
            optionsBuilder.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString)
            );

            return new TenantDbContext(optionsBuilder.Options);
        }

        // بتنشئ داتا بيس جديد وتطبق كل الجداول تلقائياً
        // بتشتغل مرة وحدة لما مركز جديد ينضاف
        public async Task CreateTenantDatabaseAsync(int centerId)
        {
            using var tenantDb = CreateForCenter(centerId);

            // هاد السطر هو اللي بيعمل الداتا بيس وكل الجداول
            // بيشوف الـ Migrations اللي عملناها ويطبقها
            await tenantDb.Database.MigrateAsync();
        }
    }
}

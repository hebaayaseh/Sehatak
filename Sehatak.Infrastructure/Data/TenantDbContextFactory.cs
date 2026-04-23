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

        public TenantDbContext CreateForCenter(int centerId)
        {
            var connectionString = BuildConnectionString(centerId);

            var optionsBuilder = new DbContextOptionsBuilder<TenantDbContext>();
            optionsBuilder.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString)
            );

            return new TenantDbContext(optionsBuilder.Options);
        }

        public async Task CreateTenantDatabaseAsync(int centerId)
        {
            var connectionString = BuildConnectionString(centerId);

            // إنشاء DB إذا مش موجودة
            await EnsureDatabaseCreated(connectionString);

            using var context = CreateForCenter(centerId);

            // تطبيق كل الـ migrations
            await context.Database.MigrateAsync();
        }

        private string BuildConnectionString(int centerId)
        {
            var dbName = $"sehatak_center_{centerId}";
            var template = _config.GetConnectionString("TenantDbTemplate");

            if (string.IsNullOrEmpty(template))
                throw new Exception("TenantDbTemplate is missing");

            return $"{template}Database={dbName};";
        }

        private async Task EnsureDatabaseCreated(string connectionString)
        {
            var builder = new MySqlConnector.MySqlConnectionStringBuilder(connectionString);
            var databaseName = builder.Database;

            // نشيل اسم الداتا بيس مؤقتاً
            builder.Database = "";

            using var connection = new MySqlConnector.MySqlConnection(builder.ConnectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = $"CREATE DATABASE IF NOT EXISTS `{databaseName}`;";
            await command.ExecuteNonQueryAsync();
        }
    }
}


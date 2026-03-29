using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace Sehatak.Infrastructure.Data
{
    public class TenantDbContextAccessor
    {
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantDbContextAccessor(
            IConfiguration config,
            IHttpContextAccessor httpContextAccessor)
        {
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }

        // بتجيب الـ TenantDbContext للمركز الحالي
        public TenantDbContext GetCurrentTenantDb()
        {
            // بنجيب الـ CenterId من الـ JWT Token
            var centerId = GetCenterIdFromToken();

            // بنبني الـ Connection String للمركز
            var connectionString = BuildConnectionString(centerId);

            var optionsBuilder = new DbContextOptionsBuilder<TenantDbContext>();
            optionsBuilder.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString)
            );

            return new TenantDbContext(optionsBuilder.Options);
        }

        // بتجيب الـ CenterId من الـ JWT Token
        private int GetCenterIdFromToken()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null)
                throw new UnauthorizedAccessException();

            // بنجيب الـ CenterId من الـ Claims
            var centerIdClaim = user.FindFirst("CenterId")?.Value;

            if (string.IsNullOrEmpty(centerIdClaim))
                throw new UnauthorizedAccessException();

            return int.Parse(centerIdClaim);
        }

        // بتبني الـ Connection String للمركز
        private string BuildConnectionString(int centerId)
        {
            var dbName = $"sehatak_center_{centerId}";
            var template = _config.GetConnectionString("TenantDbTemplate")!;
            return $"{template}Database={dbName};";
        }
    }
}

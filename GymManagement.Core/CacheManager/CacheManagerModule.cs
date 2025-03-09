using GymManagement.Core.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagement.Core.CacheManager
{
    public static class CachingModule
    {
        public static IServiceCollection RegisterCachingOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection(AppSettings.CachingOptions);
            if (section == null)
                throw new ApplicationException($"Section '{AppSettings.CachingOptions}' not found in appsettings.json.");

            return services.Configure<CachingOptions>(section.Bind);
        }
    }
}

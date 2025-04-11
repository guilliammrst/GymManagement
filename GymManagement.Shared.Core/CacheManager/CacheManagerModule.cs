using GymManagement.Shared.Core.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagement.Shared.Core.CacheManager
{
    public static class CachingModule
    {
        public static IServiceCollection ConfigureCachingOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection(nameof(CachingOptions));
            if (section == null)
                throw new ApplicationException($"Section '{nameof(CachingOptions)}' not found in appsettings.json.");

            return services.Configure<CachingOptions>(section.Bind);
        }
    }
}

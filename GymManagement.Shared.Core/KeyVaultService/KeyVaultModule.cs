using GymManagement.Shared.Core.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagement.Shared.Core.KeyVaultService
{
    public static class KeyVaultModule
    {
        public static IServiceCollection RegisterKeyVaultService(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection(AppSettings.KeyVaultOptions);
            if (section == null)
                throw new ApplicationException($"Section '{AppSettings.KeyVaultOptions}' not found in appsettings.json.");

            services.Configure<KeyVaultOptions>(section.Bind);

            return services.AddScoped<IKeyVaultService, KeyVaultService>();
        }
    }
}

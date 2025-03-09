using Microsoft.Extensions.DependencyInjection;
using GymManagement.Repositories.Modules;
using GymManagement.Services.Auth;
using GymManagement.Core.Configurations;
using Microsoft.Extensions.Configuration;
using GymManagement.Services.Auth.Interfaces;

namespace GymManagement.Services.Modules
{
    public static class AuthServiceModule
    {
        public static IServiceCollection RegisterAuthService(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection(AppSettings.IssuerOptions);
            if (section == null)
                throw new ApplicationException($"Section '{AppSettings.IssuerOptions}' not found in appsettings.json.");

            services.Configure<IssuerOptions>(section.Bind);

            return services.RegisterAuthRepository()
                .AddTransient<IAuthService, AuthService>();
        }
    }
}

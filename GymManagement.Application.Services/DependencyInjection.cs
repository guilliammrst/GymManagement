using Microsoft.Extensions.DependencyInjection;
using GymManagement.Application.Services.Auth;
using GymManagement.Shared.Core.Configurations;
using Microsoft.Extensions.Configuration;
using GymManagement.Application.Interfaces.Services.Auth;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Application.Services.Users;

namespace GymManagement.Application.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            return services.RegisterAuthService(configuration)
                .RegisterServices();
        }

        private static IServiceCollection RegisterAuthService(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection(AppSettings.IssuerOptions);
            if (section == null)
                throw new ApplicationException($"Section '{AppSettings.IssuerOptions}' not found in appsettings.json.");

            services.Configure<IssuerOptions>(section.Bind);

            return services.AddTransient<IAuthService, AuthService>();
        }

        private static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            return services.AddTransient<IUserService, UserService>();
        }
    }
}

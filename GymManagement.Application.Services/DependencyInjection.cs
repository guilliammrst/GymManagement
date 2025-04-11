using Microsoft.Extensions.DependencyInjection;
using GymManagement.Application.Services.Auth;
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
                .RegisterUserServices();
        }

        private static IServiceCollection RegisterAuthService(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddTransient<IAuthService, AuthService>();
        }

        private static IServiceCollection RegisterUserServices(this IServiceCollection services)
        {
            return services.AddTransient<IUserService, UserService>();
        }
    }
}

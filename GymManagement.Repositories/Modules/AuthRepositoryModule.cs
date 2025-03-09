using Microsoft.Extensions.DependencyInjection;
using GymManagement.Repositories.Auth;
using GymManagement.Repositories.Auth.Interfaces;

namespace GymManagement.Repositories.Modules
{
    public static class AuthRepositoryModule
    {
        public static IServiceCollection RegisterAuthRepository(this IServiceCollection services)
        {
            return services.AddScoped<IAuthRepository, AuthRepository>();
        }
    }
}

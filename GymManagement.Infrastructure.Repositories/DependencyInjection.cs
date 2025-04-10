using Microsoft.Extensions.DependencyInjection;
using GymManagement.Infrastructure.Repositories.Auth;
using GymManagement.Application.Interfaces.Repositories.Auth;
using GymManagement.Application.Interfaces.Repositories.Users;
using GymManagement.Infrastructure.Repositories.DbContexts;
using GymManagement.Infrastructure.Repositories.Users;

namespace GymManagement.Infrastructure.Repositories
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            return services.RegisterAuthRepository()
                .RegisterRepositories();
        }

        private static IServiceCollection RegisterAuthRepository(this IServiceCollection services)
        {
            return services.AddScoped<IAuthRepository, AuthRepository>();
        }

        private static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            return services.AddDbContext<GymDbContext>()
                .AddScoped<IUserRepository, UserRepository>()
                .Decorate<IUserRepository, CacheUserRepository>();
        }
    }
}

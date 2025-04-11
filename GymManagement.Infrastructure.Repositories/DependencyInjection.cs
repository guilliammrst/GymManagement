using Microsoft.Extensions.DependencyInjection;
using GymManagement.Infrastructure.Repositories.Auth;
using GymManagement.Application.Interfaces.Repositories.Auth;
using GymManagement.Application.Interfaces.Repositories.Users;
using GymManagement.Infrastructure.Repositories.DbContexts;
using GymManagement.Infrastructure.Repositories.Users;
using GymManagement.Shared.Core.Environments;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Core.Configurations;

namespace GymManagement.Infrastructure.Repositories
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            return services.AddDbSettingsConfiguration()
                .RegisterDbContext()
                .RegisterAuthRepository()
                .RegisterUserRepositories();
        }

        private static IServiceCollection RegisterAuthRepository(this IServiceCollection services)
        {
            return services.AddScoped<IAuthRepository, AuthRepository>();
        }

        private static IServiceCollection RegisterUserRepositories(this IServiceCollection services)
        {
            return services.AddScoped<IUserRepository, UserRepository>()
                .Decorate<IUserRepository, CacheUserRepository>();
        }

        private static IServiceCollection RegisterDbContext(this IServiceCollection services)
        {
            return services.AddDbContext<GymDbContext>();
        }

        private static IServiceCollection AddDbSettingsConfiguration(this IServiceCollection services)
        {
            return services.Configure<DbSettings>(options =>
            {
                options.ConnectionString = EnvironmentVariables.GetEnvironmentVariable(KeyVaultKeyNames.GymDbConnection);
            });
        }
    }
}

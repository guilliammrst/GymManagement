using GymManagement.Repositories.DbContexts;
using GymManagement.Repositories.Users;
using GymManagement.Repositories.Users.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagement.Repositories.Modules
{
    public static class RepositoriesModule
    {
        public static IServiceCollection RegisterRepositories (this IServiceCollection services)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            return services.AddDbContext<GymDbContext>()
                .AddScoped<IUserRepository, UserRepository>()
                .Decorate<IUserRepository, CacheUserRepository>();
        }
    }
}

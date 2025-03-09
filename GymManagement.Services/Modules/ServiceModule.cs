using GymManagement.Repositories.Modules;
using GymManagement.Services.Users;
using GymManagement.Services.Users.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagement.Services.Modules
{
    public static class ServiceModule
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            return services.RegisterRepositories()
                .AddTransient<IUserService, UserService>();
        }
    }
}

using GymManagement.Application.Interfaces.Repositories.Addresses;
using GymManagement.Application.Interfaces.Repositories.Auth;
using GymManagement.Application.Interfaces.Repositories.Clubs;
using GymManagement.Application.Interfaces.Repositories.CoachingPlans;
using GymManagement.Application.Interfaces.Repositories.MembershipPlans;
using GymManagement.Application.Interfaces.Repositories.Memberships;
using GymManagement.Application.Interfaces.Repositories.Users;
using GymManagement.Infrastructure.Repositories.Addresses;
using GymManagement.Infrastructure.Repositories.Auth;
using GymManagement.Infrastructure.Repositories.Clubs;
using GymManagement.Infrastructure.Repositories.CoachingPlans;
using GymManagement.Infrastructure.Repositories.DbContexts;
using GymManagement.Infrastructure.Repositories.MembershipPlans;
using GymManagement.Infrastructure.Repositories.Memberships;
using GymManagement.Infrastructure.Repositories.Users;
using GymManagement.Shared.Core.Configurations;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Core.Environments;
using Microsoft.Extensions.DependencyInjection;

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
                .RegisterUserRepositories()
                .RegisterClubRepositories()
                .RegisterAddressRepositories()
                .RegisterMembershipPlanRepositories()
                .RegisterMembershipRepositories()
                .RegisterCoachingPlanRepositories();
        }

        private static IServiceCollection RegisterCoachingPlanRepositories(this IServiceCollection services)
        {
            return services.AddScoped<ICoachingPlanQueryRepository, CoachingPlanQueryRepository>()
                .AddScoped<ICoachingPlanCommandRepository, CoachingPlanCommandRepository>();
        }

        private static IServiceCollection RegisterMembershipRepositories(this IServiceCollection services)
        {
            return services.AddScoped<IMembershipQueryRepository, MembershipQueryRepository>();
        }

        private static IServiceCollection RegisterMembershipPlanRepositories(this IServiceCollection services)
        {
            return services.AddScoped<IMembershipPlanQueryRepository, MembershipPlanQueryRepository>()
                .AddScoped<IMembershipPlanCommandRepository, MembershipPlanCommandRepository>();
        }

        private static IServiceCollection RegisterAddressRepositories(this IServiceCollection services)
        {
            return services.AddScoped<IAddressCommandRepository, AddressCommandRepository>();
        }

        private static IServiceCollection RegisterClubRepositories(this IServiceCollection services)
        {
            return services.AddScoped<IClubQueryRepository, ClubQueryRepository>()
                .AddScoped<IClubCommandRepository, ClubCommandRepository>();
        }

        private static IServiceCollection RegisterAuthRepository(this IServiceCollection services)
        {
            return services.AddScoped<IAuthRepository, AuthRepository>();
        }

        private static IServiceCollection RegisterUserRepositories(this IServiceCollection services)
        {
            return services.AddScoped<IUserQueryRepository, UserQueryRepository>()
                .AddScoped<IUserCommandRepository, UserCommandRepository>()
                .AddScoped<IUserSubscriptionRepository, UserSubscriptionRepository>();
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

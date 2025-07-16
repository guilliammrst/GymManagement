using Microsoft.Extensions.DependencyInjection;
using GymManagement.Application.Services.Auth;
using Microsoft.Extensions.Configuration;
using GymManagement.Application.Interfaces.Services.Auth;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Application.Services.Users;
using GymManagement.Application.Services.Clubs;
using GymManagement.Application.Interfaces.Services.Clubs;
using GymManagement.Application.Interfaces.Services.MembershipPlans;
using GymManagement.Application.Services.MembershipPlans;
using GymManagement.Application.Interfaces.Services.QrCodes;
using GymManagement.Application.Services.QrCodes;
using GymManagement.Application.Interfaces.Services.Memberships;
using GymManagement.Application.Services.Memberships;
using GymManagement.Application.Interfaces.Services.CoachingPlans;
using GymManagement.Application.Services.CoachingPlans;

namespace GymManagement.Application.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            return services.RegisterAuthService(configuration)
                .RegisterUserServices()
                .RegisterClubServices()
                .RegisterMembershipServices()
                .RegisterMembershipPlanServices()
                .RegisterQrCodeServices()
                .RegisterCoachingPlanServices();
        }

        private static IServiceCollection RegisterCoachingPlanServices(this IServiceCollection services)
        {
            return services.AddTransient<ICoachingPlanQueryService, CoachingPlanQueryService>()
                .AddTransient<ICoachingPlanCommandService, CoachingPlanCommandService>();
        }

        private static IServiceCollection RegisterQrCodeServices(this IServiceCollection services)
        {
            return services.AddTransient<IQrCodeGenerationService, QrCodeGenerationService>()
                .AddTransient<IQrCodeValidationService, QrCodeValidationService>();
        }

        private static IServiceCollection RegisterMembershipServices(this IServiceCollection services)
        {
            return services.AddTransient<IMembershipQueryService, MembershipQueryService>();
        }

        private static IServiceCollection RegisterMembershipPlanServices(this IServiceCollection services)
        {
            return services.AddTransient<IMembershipPlanCommandService, MembershipPlanCommandService>()
                .AddTransient<IMembershipPlanQueryService, MembershipPlanQueryService>();
        }

        private static IServiceCollection RegisterClubServices(this IServiceCollection services)
        {
            return services.AddTransient<IClubCommandService, ClubCommandService>()
                .AddTransient<IClubQueryService, ClubQueryService>();
        }

        private static IServiceCollection RegisterAuthService(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddTransient<IAuthService, AuthService>();
        }

        private static IServiceCollection RegisterUserServices(this IServiceCollection services)
        {
            return services.AddTransient<IUserQueryService, UserQueryService>()
                .AddTransient<IUserCommandService, UserCommandService>()
                .AddTransient<IUserSubscriptionService, UserSubscriptionService>()
                .AddTransient<IUserVerificationService, UserVerificationService>();
        }
    }
}

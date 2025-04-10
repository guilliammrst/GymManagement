using GymManagement.Shared.Core.Configurations;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Core.Identity;
using Microsoft.AspNetCore.Authorization;

namespace GymManagement.Presentation.Api.Configurations
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection ConfigureSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection(AppSettings.IssuerOptions);
            if (section == null)
                throw new ApplicationException($"Section '{AppSettings.IssuerOptions}' not found in appsettings.json.");
            
            services.Configure<IssuerOptions>(section.Bind);

            services.AddAuthentication().AddJwtBearer();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(RoleConstants.None, policy => policy.RequireRole(RoleConstants.None));
                options.AddPolicy(RoleConstants.Member, policy => policy.RequireRole(RoleConstants.Member));
                options.AddPolicy(RoleConstants.Coach, policy => policy.RequireRole(RoleConstants.Coach));
                options.AddPolicy(RoleConstants.Staff, policy => policy.RequireRole(RoleConstants.Staff));
                options.AddPolicy(RoleConstants.Manager, policy => policy.RequireRole(RoleConstants.Manager));
            });

            return services.AddScoped<IAuthorizationHandler, GymAuthorizationHandler>()
                .AddHttpContextAccessor();
        }
    }
}

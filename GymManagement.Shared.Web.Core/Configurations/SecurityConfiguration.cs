using GymManagement.Shared.Core.Configurations;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Core.Environments;
using GymManagement.Shared.Core.JwtValidation;
using GymManagement.Shared.Web.Core.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagement.Shared.Web.Core.Configurations
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection ConfigureSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            var issuer = EnvironmentVariables.GetEnvironmentVariable(KeyVaultKeyNames.JwtIssuer);
            var audience = EnvironmentVariables.GetEnvironmentVariable(KeyVaultKeyNames.JwtAudience);
            var secretKey = EnvironmentVariables.GetEnvironmentVariable(KeyVaultKeyNames.JwtSecretKey);

            services.Configure<IssuerOptions>(options =>
            {
                options.Issuer = issuer;
                options.Audience = audience;
                options.SecretKey = secretKey;
            });

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
                .AddScoped<IJwtValidationService, JwtValidationService>()
                .AddHttpContextAccessor();
        }
    }
}

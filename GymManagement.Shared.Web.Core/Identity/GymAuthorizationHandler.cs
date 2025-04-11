using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GymManagement.Shared.Core.Configurations;
using GymManagement.Shared.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GymManagement.Shared.Web.Core.Identity
{
    public class GymAuthorizationHandler(IHttpContextAccessor _httpContextAccessor, IOptions<IssuerOptions> options) : IAuthorizationHandler
    {
        private readonly IssuerOptions _issuerOptions = options.Value;

        private static void ValidateRequirements(AuthorizationHandlerContext context, ClaimsPrincipal principal)
        {
            var validateRequirements = new List<IAuthorizationRequirement>();
            foreach (var requirement in context.Requirements)
            {
                if (requirement is RolesAuthorizationRequirement roleRequirement)
                {
                    foreach (var role in roleRequirement.AllowedRoles)
                    {
                        if (principal.IsInRole(role))
                        {
                            context.Succeed(requirement);
                            validateRequirements.Add(requirement);
                        }
                    }
                }
            }

            if (validateRequirements.Count == 0)
            {
                context.Fail();
            }
        }

        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            var token = GetBearerToken();
            if (string.IsNullOrEmpty(token))
            {
                context.Fail();
                return Task.CompletedTask;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_issuerOptions.SecretKey));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _issuerOptions.Issuer,
                ValidAudience = _issuerOptions.Audience,
                IssuerSigningKey = key,
                RoleClaimType = ClaimsTypes.Role
            };
            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                _httpContextAccessor.HttpContext.User = principal;
                ValidateRequirements(context, principal);
            }
            catch
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }

        private string? GetBearerToken()
        {
            var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return null;
            }
            var header = authorizationHeader.ToString();
            if (header.StartsWith("Bearer "))
            {
                return header.Substring(7);
            }
            return null;
        }
    }
}

using System.Security.Claims;
using GymManagement.Shared.Core.Configurations;
using GymManagement.Shared.Core.JwtValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace GymManagement.Shared.Web.Core.Identity
{
    public class GymAuthorizationHandler(IHttpContextAccessor _httpContextAccessor, IJwtValidationService _jwtValidationService, IOptions<IssuerOptions> options) : IAuthorizationHandler
    {
        private readonly IssuerOptions _issuerOptions = options.Value;
        private const string REFRESH_TOKEN_PATH = "/api/refresh-token";

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
                context.Fail();
        }

        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            var token = GetBearerToken();
            if (string.IsNullOrEmpty(token))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var validateLifetime = true;
            if (context.Resource is HttpContext httpContext)
            {
                if (httpContext.Request.Path.Value == REFRESH_TOKEN_PATH)
                    validateLifetime = false;
            }

            var result = _jwtValidationService.ValidateToken(token, validateLifetime);
            if (!result.Success)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var claimsPrincipal = result.Results;
            
            _httpContextAccessor.HttpContext.User = claimsPrincipal;
            ValidateRequirements(context, claimsPrincipal);
            
            return Task.CompletedTask;
        }

        private string? GetBearerToken()
        {
            var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authorizationHeader))
                return null;

            var header = authorizationHeader.ToString();
            if (header.StartsWith("Bearer "))
                return header.Substring(7);

            return null;
        }
    }
}

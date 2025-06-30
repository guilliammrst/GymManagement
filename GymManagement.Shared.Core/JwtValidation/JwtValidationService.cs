using GymManagement.Shared.Core.Configurations;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GymManagement.Shared.Core.JwtValidation
{
    public class JwtValidationService(IOptions<IssuerOptions> _options) : IJwtValidationService
    {
        private readonly IssuerOptions _issuerOptions = _options.Value;

        public ModelActionResult<ClaimsPrincipal> ValidateToken(string jwtToken, bool validateLifetime = true)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_issuerOptions.SecretKey));
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = validateLifetime,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _issuerOptions.Issuer,
                    ValidAudience = _issuerOptions.Audience,
                    IssuerSigningKey = key,
                    RoleClaimType = ClaimsTypes.Role
                };
                var principal = tokenHandler.ValidateToken(jwtToken, validationParameters, out var validatedToken);
                return ModelActionResult<ClaimsPrincipal>.Ok(principal);
            }
            catch (Exception ex)
            {
                return ModelActionResult<ClaimsPrincipal>.Fail(GymFaultType.TokenValidationFailed, ex.Message);
            }
        }
    }
}

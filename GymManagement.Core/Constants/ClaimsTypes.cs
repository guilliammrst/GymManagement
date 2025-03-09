using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GymManagement.Core.Constants
{
    public class ClaimsTypes
    {
        public const string Jti = JwtRegisteredClaimNames.Jti;
        public const string Role = ClaimTypes.Role;
        public const string Email = JwtRegisteredClaimNames.Sub;
    }
}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GymManagement.Shared.Core.Constants
{
    public class ClaimsTypes
    {
        public const string Jti = JwtRegisteredClaimNames.Jti;
        public const string Role = ClaimTypes.Role;
        public const string Email = JwtRegisteredClaimNames.Sub;
        public const string NameIdentifier = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
    }
}

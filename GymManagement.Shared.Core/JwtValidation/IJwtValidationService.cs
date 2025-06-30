using GymManagement.Shared.Core.Results;
using System.Security.Claims;

namespace GymManagement.Shared.Core.JwtValidation
{
    public interface IJwtValidationService
    {
        ModelActionResult<ClaimsPrincipal> ValidateToken(string jwtToken, bool validateLifetime = true);
    }
}

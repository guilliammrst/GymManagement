using System.Security.Claims;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;

namespace GymManagement.Shared.Core.ClaimsPrincipalExt
{
    public static class ClaimsPrincipalExt
    {
        public static ModelActionResult<string> GetEmail(this ClaimsPrincipal claimsPrincipal)
        {
            var emailClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimsTypes.Email || c.Type == ClaimsTypes.NameIdentifier);
            if (emailClaim == null)
                return ModelActionResult<string>.Fail(GymFaultType.GetEmailFromTokenFailed, "Fail to retrieve email from token.");

            return ModelActionResult<string>.Ok(emailClaim.Value);
        }
    }
}

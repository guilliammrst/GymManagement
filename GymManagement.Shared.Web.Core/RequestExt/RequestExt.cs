using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;
using Microsoft.AspNetCore.Http;

namespace GymManagement.Shared.Web.Core.RequestExt
{
    public static class RequestExt
    {
        public static ModelActionResult<string> GetToken(this HttpRequest httpRequest)
        {
            var authHeader = httpRequest.Headers.Authorization.ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return ModelActionResult<string>.Fail(GymFaultType.GetTokenFailed, "Get token failed.");

            var token = authHeader["Bearer ".Length..].Trim();

            if (string.IsNullOrEmpty(token))
                return ModelActionResult<string>.Fail(GymFaultType.GetTokenFailed, "Get token failed.");

            return ModelActionResult<string>.Ok(token);
        }
    }
}

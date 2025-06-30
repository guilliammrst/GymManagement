using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Core.Enums;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using GymManagement.Shared.Core.Results;
using GymManagement.Presentation.WebApp.ApiClients.Auth;

namespace GymManagement.Presentation.WebApp
{
    public class AuthManager (NavigationManager _navigationManager, AuthenticatedUser _authenticatedUser, AuthApiClient _authApiClient) : IAuthManager
    {
        public void RedirectToLogin(bool forceLoad = false)
        {
            _navigationManager.NavigateTo("/login", forceLoad: forceLoad);
        }

        public void RedirectToHome()
        {
            _navigationManager.NavigateTo("/");
        }

        public bool Login(string token)
        {
            var claims = ParseClaimsFromJwt(token);
            var role = claims.Where(c => c.Type == ClaimsTypes.Role).FirstOrDefault();
            if (role == null || (role.Value != Role.Staff.ToString() && role.Value != Role.Manager.ToString()))
                return false;

            _authenticatedUser.Token = token;
            _authenticatedUser.Email = claims.Where(c => c.Type == ClaimsTypes.Email).FirstOrDefault()?.Value!;
            _authenticatedUser.Role = Enum.Parse<Role>(role.Value);

            var expClaim = claims.FirstOrDefault(c => c.Type == "exp");
            if (expClaim != null && long.TryParse(expClaim.Value, out long expSeconds))
            {
                _authenticatedUser.TokenExpiration = DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime;
            }

            return true;
        }

        public async Task<ModelActionResult> RefreshToken()
        {
            var refreshTokenResult = await _authApiClient.RefreshToken();
            if (!refreshTokenResult.Success)
                return ModelActionResult.Fail(refreshTokenResult);

            var token = refreshTokenResult.Results;

            if (!Login(token))
                return ModelActionResult.Fail(GymFaultType.UserNotAuthenticated);

            return ModelActionResult.Ok;
        }

        public void Logout()
        {
            _authenticatedUser.Token = string.Empty;
            _authenticatedUser.Email = string.Empty;
            _authenticatedUser.Role = Role.None;
            RedirectToLogin(true);
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];

            payload = payload.Replace('-', '+').Replace('_', '/');
            
            int mod4 = payload.Length % 4;
            if (mod4 > 0)
            {
                payload = payload.PadRight(payload.Length + (4 - mod4), '=');
            }

            var jsonBytes = Convert.FromBase64String(payload);
            var claims = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            return claims.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }
    }
}

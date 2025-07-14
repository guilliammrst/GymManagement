using GymManagement.Shared.Core.AuthManager;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;
using System.Security.Claims;
using System.Text.Json;

namespace GymManagement.Presentation.MobileApp
{
    public class AuthManager(AuthenticatedUser _authenticatedUser, IPreferencesService _preferencesService, AuthApiClient _authApiClient) : IAuthManager
    {
        public async void RedirectToLogin(bool forceLoad = false)
        {
            if (Shell.Current == null)
                return;

            await Shell.Current.GoToAsync("//LoginPage", true);
        }

        public async void RedirectToHome()
        {
            if (Shell.Current == null)
                return;

            await Shell.Current.GoToAsync("//MainPage");
        }

        public async Task<ModelActionResult> Login(LoginDto loginDto)
        {
            var tokenResult = await _authApiClient.Login(loginDto);
            if (!tokenResult.Success)
                return ModelActionResult.Fail(tokenResult);

            var token = tokenResult.Results;

            var loginResult = LogUser(token);
            if (!loginResult.Success)
                return ModelActionResult.Fail(loginResult);

            _preferencesService.SaveUserToPreferences();

            return ModelActionResult.Ok;
        }

        public async Task<ModelActionResult> Register(RegisterDto registerDto)
        {
            var registerResult = await _authApiClient.Register(registerDto);
            if (!registerResult.Success)
                return ModelActionResult.Fail(registerResult);

            var token = registerResult.Results;

            var loginResult = LogUser(token);
            if (!loginResult.Success)
                return ModelActionResult.Fail(loginResult);

            _preferencesService.SaveUserToPreferences();

            return ModelActionResult.Ok;
        }

        public async Task<ModelActionResult> RefreshToken()
        {
            var refreshTokenResult = await _authApiClient.RefreshToken();
            if (!refreshTokenResult.Success)
                return ModelActionResult.Fail(refreshTokenResult);

            var token = refreshTokenResult.Results;

            var loginResult = LogUser(token);
            if (!loginResult.Success)
                return ModelActionResult.Fail(loginResult);

            _preferencesService.SaveUserToPreferences();

            return ModelActionResult.Ok;
        }

        public void Logout()
        {
            _authenticatedUser.Token = string.Empty;
            _authenticatedUser.Email = string.Empty;
            _authenticatedUser.Role = Role.None;
            _authenticatedUser.TokenExpiration = null;
            _preferencesService.DeleteUserFromPreferences();
            RedirectToLogin(true);
        }

        private ModelActionResult LogUser(string token)
        {
            var claims = ParseClaimsFromJwt(token);
            var role = claims.Where(c => c.Type == ClaimsTypes.Role).FirstOrDefault();
            if (role == null)
                role = new Claim(ClaimsTypes.Role, Role.None.ToString());

            _authenticatedUser.Token = token;
            _authenticatedUser.Email = claims.Where(c => c.Type == ClaimsTypes.Email).FirstOrDefault()?.Value!;
            _authenticatedUser.Role = Enum.Parse<Role>(role.Value);

            var expClaim = claims.FirstOrDefault(c => c.Type == "exp");
            if (expClaim != null && long.TryParse(expClaim.Value, out long expSeconds))
            {
                _authenticatedUser.TokenExpiration = DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime;
            }

            return ModelActionResult.Ok;
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

using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Core.Enums;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using GymManagement.Shared.Core.Results;
using GymManagement.Shared.Core.AuthManager;

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

        public async Task<ModelActionResult> Login(LoginDto loginDto)
        {
            try
            {
                Console.WriteLine($"🔍 AuthManager.Login - Début pour: {loginDto.Email}");
                
                var tokenResult = await _authApiClient.Login(loginDto);
                Console.WriteLine($"🔍 AuthManager.Login - Token result success: {tokenResult.Success}");
                
                if (!tokenResult.Success)
                {
                    Console.WriteLine($"🔍 AuthManager.Login - Échec du token: {tokenResult.Fault}");
                    return ModelActionResult.Fail(tokenResult);
                }

                var token = tokenResult.Results;
                Console.WriteLine($"🔍 AuthManager.Login - Token reçu: {token?.Substring(0, Math.Min(50, token?.Length ?? 0))}...");

                var loginResult = LogUser(token);
                Console.WriteLine($"🔍 AuthManager.Login - LogUser result: {loginResult.Success}");
                
                if (!loginResult.Success)
                {
                    Console.WriteLine($"🔍 AuthManager.Login - Échec LogUser: {loginResult.Fault}");
                    return ModelActionResult.Fail(loginResult);
                }

                Console.WriteLine($"🔍 AuthManager.Login - Succès! User authentifié: {_authenticatedUser.IsAuthenticated}");
                return ModelActionResult.Ok;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔍 AuthManager.Login - Exception: {ex.Message}");
                Console.WriteLine($"🔍 AuthManager.Login - StackTrace: {ex.StackTrace}");
                return ModelActionResult.Fail(GymFaultType.GetTokenFailed, ex.Message);
            }
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

            return ModelActionResult.Ok;
        }

        public void Logout()
        {
            Console.WriteLine("🔍 AuthManager.Logout - Déconnexion");
            _authenticatedUser.Token = string.Empty;
            _authenticatedUser.Email = string.Empty;
            _authenticatedUser.Role = Role.None;
            _authenticatedUser.TokenExpiration = null;
            RedirectToLogin(true);
        }

        public async Task<ModelActionResult> Register(RegisterDto registerDto)
        {
            // Register method is not implemented for web app
            return ModelActionResult.Ok;
        }

        private ModelActionResult LogUser(string token)
        {
            try
            {
                Console.WriteLine($"🔍 AuthManager.LogUser - Début parsing du token");
                
                var claims = ParseClaimsFromJwt(token);
                var claimsList = claims.ToList();
                
                Console.WriteLine($"🔍 AuthManager.LogUser - Claims trouvés: {claimsList.Count}");
                foreach (var claim in claimsList)
                {
                    Console.WriteLine($"🔍 Claim: {claim.Type} = {claim.Value}");
                }
                
                var role = claimsList.Where(c => c.Type == ClaimsTypes.Role).FirstOrDefault();
                Console.WriteLine($"🔍 AuthManager.LogUser - Role claim: {role?.Value}");
                
                if (role == null)
                {
                    Console.WriteLine($"🔍 AuthManager.LogUser - Aucun rôle trouvé dans le token");
                    return ModelActionResult.Fail(GymFaultType.UserNotAuthorized, "Aucun rôle trouvé dans le token");
                }
                
                if (role.Value != Role.Coach.ToString() && 
                    role.Value != Role.Staff.ToString() && 
                    role.Value != Role.Manager.ToString())
                {
                    Console.WriteLine($"🔍 AuthManager.LogUser - Rôle non autorisé: {role.Value}");
                    return ModelActionResult.Fail(GymFaultType.UserNotAuthorized, $"Rôle {role.Value} non autorisé pour l'application Web");
                }

                _authenticatedUser.Token = token;
                _authenticatedUser.Email = claimsList.Where(c => c.Type == ClaimsTypes.Email).FirstOrDefault()?.Value ?? "";
                _authenticatedUser.Role = Enum.Parse<Role>(role.Value);

                var expClaim = claimsList.FirstOrDefault(c => c.Type == "exp");
                if (expClaim != null && long.TryParse(expClaim.Value, out long expSeconds))
                {
                    _authenticatedUser.TokenExpiration = DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime;
                }

                Console.WriteLine($"🔍 AuthManager.LogUser - Utilisateur connecté: Email={_authenticatedUser.Email}, Role={_authenticatedUser.Role}, IsAuthenticated={_authenticatedUser.IsAuthenticated}");
                
                return ModelActionResult.Ok;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔍 AuthManager.LogUser - Exception: {ex.Message}");
                Console.WriteLine($"🔍 AuthManager.LogUser - StackTrace: {ex.StackTrace}");
                return ModelActionResult.Fail(GymFaultType.UserNotAuthorized, $"Erreur lors du parsing du token: {ex.Message}");
            }
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            try
            {
                var parts = jwt.Split('.');
                if (parts.Length != 3)
                {
                    throw new ArgumentException("Token JWT invalide - doit avoir 3 parties");
                }
                
                var payload = parts[1];

                payload = payload.Replace('-', '+').Replace('_', '/');
                
                int mod4 = payload.Length % 4;
                if (mod4 > 0)
                {
                    payload = payload.PadRight(payload.Length + (4 - mod4), '=');
                }

                var jsonBytes = Convert.FromBase64String(payload);
                var jsonString = System.Text.Encoding.UTF8.GetString(jsonBytes);
                Console.WriteLine($"🔍 JWT Payload JSON: {jsonString}");
                
                var claims = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
                
                if (claims == null)
                {
                    throw new ArgumentException("Impossible de désérialiser les claims du token");
                }

                return claims.Select(kvp => new Claim(kvp.Key, kvp.Value?.ToString() ?? ""));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔍 ParseClaimsFromJwt - Exception: {ex.Message}");
                throw;
            }
        }
    }
}

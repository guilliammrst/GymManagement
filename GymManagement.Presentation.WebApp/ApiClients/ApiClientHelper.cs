using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;
using GymManagement.Shared.Web.Core.Controllers;

namespace GymManagement.Presentation.WebApp.ApiClients
{
    public class ApiClientHelper(AuthenticatedUser _authenticatedUser, IAuthManager _authManager) : IApiClientHelper
    {
        public async Task<ModelActionResult> GenerateActionResult(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorDto = await response.Content.ReadFromJsonAsync<ErrorDto>();
                return ModelActionResult.Fail((GymFaultType)errorDto?.FaultCode!, errorDto?.Message!);
            }
            return ModelActionResult.Ok;
        }

        public async Task<ModelActionResult> CheckAuthenticatedUser()
        {
            if (!_authenticatedUser.IsAuthenticated)
                return ModelActionResult.Fail(GymFaultType.UserNotAuthenticated);

            if (_authenticatedUser.IsTokenExpired)
            {
                var result = await _authManager.RefreshToken();
                if (!result.Success)
                {
                    _authenticatedUser.Token = string.Empty;
                    _authenticatedUser.TokenExpiration = null;
                    _authenticatedUser.Email = string.Empty;
                    _authenticatedUser.Role = Role.None;
                    _authManager.RedirectToLogin();
                    return ModelActionResult.Fail(GymFaultType.UserNotAuthenticated);
                }
            }
            return ModelActionResult.Ok;
        }
    }
}

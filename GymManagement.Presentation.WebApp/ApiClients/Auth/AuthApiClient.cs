using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;
using System.Net;

namespace GymManagement.Presentation.WebApp.ApiClients.Auth
{
    public class AuthApiClient(HttpClient _httpClient, AuthenticatedUser _authenticatedUser)
    {
        public async Task<ModelActionResult<string>> Login(LoginDto loginDto)
        {
            var body = new { loginDto.Email, loginDto.Password };

            var response = await _httpClient.PostAsJsonAsync("api/login", body);

            if (response.StatusCode != HttpStatusCode.OK)
                return ModelActionResult<string>.Fail(GymFaultType.GetTokenFailed);

            var token = await response.Content.ReadAsStringAsync();
            return ModelActionResult<string>.Ok(token);
        }

        public async Task<ModelActionResult<string>> RefreshToken()
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
            var response = await _httpClient.GetAsync("api/refresh-token");

            if (response.StatusCode != HttpStatusCode.OK)
                return ModelActionResult<string>.Fail(GymFaultType.RefreshTokenFailed);

            var token = await response.Content.ReadAsStringAsync();
            return ModelActionResult<string>.Ok(token);
        }
    }
}

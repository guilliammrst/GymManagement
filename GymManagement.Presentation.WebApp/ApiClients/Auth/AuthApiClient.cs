using System.Net;

namespace GymManagement.Presentation.WebApp.ApiClients.Auth
{
    public class AuthApiClient(HttpClient _httpClient)
    {
        public async Task<string?> Login(LoginDto loginDto)
        {
            var body = new { loginDto.Email, loginDto.Password };

            var response = await _httpClient.PostAsJsonAsync("api/login", body);

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            var token = await response.Content.ReadAsStringAsync();
            return token;
        }
    }
}

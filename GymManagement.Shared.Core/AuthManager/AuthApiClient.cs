using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;
using System.Net;
using System.Net.Http.Json;

namespace GymManagement.Shared.Core.AuthManager
{
    public class AuthApiClient(HttpClient _httpClient, AuthenticatedUser _authenticatedUser)
    {
        public async Task<ModelActionResult<string>> Login(LoginDto loginDto)
        {
            var body = new 
            { 
                loginDto.Email, 
                loginDto.Password 
            };

            var response = await _httpClient.PostAsJsonAsync("api/login", body);

            if (response.StatusCode != HttpStatusCode.OK)
                return ModelActionResult<string>.Fail(GymFaultType.GetTokenFailed);

            var token = await response.Content.ReadAsStringAsync();
            return ModelActionResult<string>.Ok(token);
        }

        public async Task<ModelActionResult<string>> Register(RegisterDto registerDto)
        {
            var body = new
            {
                registerDto.Email,
                registerDto.Password,
                registerDto.Name,
                registerDto.Surname,
                registerDto.Birthdate,
                registerDto.PhoneNumber,
                registerDto.Gender,
                registerDto.Country,
                registerDto.City,
                registerDto.Street,
                registerDto.PostalCode,
                registerDto.Number
            };

            var response = await _httpClient.PostAsJsonAsync("api/register", body);

            if (response.StatusCode != HttpStatusCode.Created)
            {
                var errorDto = await response.Content.ReadFromJsonAsync<ErrorDto>();
                if (errorDto != null)
                    return ModelActionResult<string>.Fail((GymFaultType)errorDto.FaultCode, errorDto.Message);

                return ModelActionResult<string>.Fail(GymFaultType.RegistrationFailed);
            }

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

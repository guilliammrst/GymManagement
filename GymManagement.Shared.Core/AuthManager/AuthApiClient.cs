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
            try
            {
                Console.WriteLine($"🔍 AuthApiClient.Login - Début pour: {loginDto.Email}");
                Console.WriteLine($"🔍 AuthApiClient.Login - URL de base: {_httpClient.BaseAddress}");
                
                var body = new 
                { 
                    loginDto.Email, 
                    loginDto.Password 
                };

                Console.WriteLine($"🔍 AuthApiClient.Login - Envoi requête POST vers: {_httpClient.BaseAddress}api/login");
                
                var response = await _httpClient.PostAsJsonAsync("api/login", body);
                
                Console.WriteLine($"🔍 AuthApiClient.Login - Status Code: {response.StatusCode}");
                Console.WriteLine($"🔍 AuthApiClient.Login - Reason Phrase: {response.ReasonPhrase}");

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"🔍 AuthApiClient.Login - Contenu erreur: {errorContent}");
                    
                    // Essayer de lire l'erreur structurée
                    try
                    {
                        var errorDto = await response.Content.ReadFromJsonAsync<ErrorDto>();
                        if (errorDto != null)
                        {
                            Console.WriteLine($"🔍 AuthApiClient.Login - Erreur structurée: {errorDto.Message}");
                            return ModelActionResult<string>.Fail((GymFaultType)errorDto.FaultCode, errorDto.Message);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"🔍 AuthApiClient.Login - Impossible de parser l'erreur: {ex.Message}");
                    }
                    
                    return ModelActionResult<string>.Fail(GymFaultType.GetTokenFailed, $"Erreur HTTP {response.StatusCode}: {response.ReasonPhrase}");
                }

                var token = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"🔍 AuthApiClient.Login - Token reçu avec succès: {token?.Substring(0, Math.Min(50, token?.Length ?? 0))}...");
                
                return ModelActionResult<string>.Ok(token);
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"🔍 AuthApiClient.Login - HttpRequestException: {httpEx.Message}");
                return ModelActionResult<string>.Fail(GymFaultType.GetTokenFailed, $"Erreur réseau: {httpEx.Message}");
            }
            catch (TaskCanceledException tcEx)
            {
                Console.WriteLine($"🔍 AuthApiClient.Login - TaskCanceledException: {tcEx.Message}");
                return ModelActionResult<string>.Fail(GymFaultType.GetTokenFailed, "Timeout - Le serveur ne répond pas");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔍 AuthApiClient.Login - Exception: {ex.Message}");
                Console.WriteLine($"🔍 AuthApiClient.Login - StackTrace: {ex.StackTrace}");
                return ModelActionResult<string>.Fail(GymFaultType.GetTokenFailed, ex.Message);
            }
        }

        public async Task<ModelActionResult<string>> Register(RegisterDto registerDto)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"🔍 AuthApiClient.Register - Exception: {ex.Message}");
                return ModelActionResult<string>.Fail(GymFaultType.RegistrationFailed, ex.Message);
            }
        }

        public async Task<ModelActionResult<string>> RefreshToken()
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
                var response = await _httpClient.GetAsync("api/refresh-token");

                if (response.StatusCode != HttpStatusCode.OK)
                    return ModelActionResult<string>.Fail(GymFaultType.RefreshTokenFailed);

                var token = await response.Content.ReadAsStringAsync();
                return ModelActionResult<string>.Ok(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔍 AuthApiClient.RefreshToken - Exception: {ex.Message}");
                return ModelActionResult<string>.Fail(GymFaultType.RefreshTokenFailed, ex.Message);
            }
        }
    }
}

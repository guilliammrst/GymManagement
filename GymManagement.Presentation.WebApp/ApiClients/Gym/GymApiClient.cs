using GymManagement.Application.Interfaces.Controllers.DTOs;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;
using GymManagement.Shared.Web.Core.Controllers;
using System.Text.Json;

namespace GymManagement.Presentation.WebApp.ApiClients.Gym
{
    public class GymApiClient(HttpClient _httpClient, AuthenticatedUser _authenticatedUser)
    {
        public async Task<UserDetailsDto?> GetUserByIdAsync(int id)
        {
            if (!_authenticatedUser.IsAuthenticated)
                return null;

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
            var response = await _httpClient.GetAsync("api/users/" + id);
            response.EnsureSuccessStatusCode();
            var user = await response.Content.ReadFromJsonAsync<UserDetailsDto>();
            return user;
        }

        public async Task<List<UserDto>> GetUsersAsync(UserFilter userFilter)
        {
            if (!_authenticatedUser.IsAuthenticated)
                return [];

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
            var response = await _httpClient.GetAsync($"api/users?page-size={userFilter.PageSize}&page-number={userFilter.PageNumber}&email={userFilter.Email}");
            response.EnsureSuccessStatusCode();
            var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();
            return users ?? [];
        }

        public async Task<ModelActionResult> CreateUserAsync(UserCreateDto userCreateDto)
        {
            if (!_authenticatedUser.IsAuthenticated)
                return ModelActionResult.Fail(GymFaultType.UserNotAuthenticated);

            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
                var response = await _httpClient.PostAsJsonAsync("api/users", userCreateDto);

                return await ApiClientHelper.GenerateActionResult(response);
            }
            catch(Exception ex)
            {
                return ModelActionResult.Fail(GymFaultType.ApiCallFailed, ex.Message);
            }
        }
    }
}
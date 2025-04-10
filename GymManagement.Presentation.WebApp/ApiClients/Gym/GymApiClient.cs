namespace GymManagement.Presentation.WebApp.ApiClients.Gym
{
    public class GymApiClient(HttpClient _httpClient, AuthenticatedUser _authenticatedUser)
    {
        public async Task<List<UserDto>> GetUsers()
        {
            if (!_authenticatedUser.IsAuthenticated)
                return new List<UserDto>();

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
            var response = await _httpClient.GetAsync("api/users");
            response.EnsureSuccessStatusCode();
            var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();
            return users ?? new List<UserDto>();
        }
    }
}
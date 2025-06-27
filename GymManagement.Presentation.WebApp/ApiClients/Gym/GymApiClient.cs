using GymManagement.Application.Interfaces.Controllers.DTOs;
using GymManagement.Application.Interfaces.Repositories.Clubs;
using GymManagement.Application.Interfaces.Services.Clubs;
using GymManagement.Application.Interfaces.Services.MembershipPlans;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;

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

        public async Task<ModelActionResult> CreateUserAsync(CreateUserDto userCreateDto)
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
            catch (Exception ex)
            {
                return ModelActionResult.Fail(GymFaultType.ApiCallFailed, ex.Message);
            }
        }

        public async Task<ModelActionResult> UpdateUserAsync(int userId, UpdateUserDto userUpdateDto)
        {
            if (!_authenticatedUser.IsAuthenticated)
                return ModelActionResult.Fail(GymFaultType.UserNotAuthenticated);

            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
                var response = await _httpClient.PatchAsJsonAsync("api/users/" + userId, userUpdateDto);

                return await ApiClientHelper.GenerateActionResult(response);
            }
            catch (Exception ex)
            {
                return ModelActionResult.Fail(GymFaultType.ApiCallFailed, ex.Message);
            }
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            if (!_authenticatedUser.IsAuthenticated)
                return false;
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
                var response = await _httpClient.DeleteAsync("api/users/" + userId);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<ModelActionResult> SubscribeUserAsync(int userId, SubscribeUserDto userSubscribeDto)
        {
            if (!_authenticatedUser.IsAuthenticated)
                return ModelActionResult.Fail(GymFaultType.UserNotAuthenticated);

            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
                var response = await _httpClient.PostAsJsonAsync("api/users/" + userId + "/subscribe", userSubscribeDto);

                return await ApiClientHelper.GenerateActionResult(response);
            }
            catch (Exception ex)
            {
                return ModelActionResult.Fail(GymFaultType.ApiCallFailed, ex.Message);
            }
        }

        public async Task<ModelActionResult> PayMembershipAsync(int userId, int membershipId, PaymentDto paymentDto)
        {
            if (!_authenticatedUser.IsAuthenticated)
                return ModelActionResult.Fail(GymFaultType.UserNotAuthenticated);

            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
                var response = await _httpClient.PostAsJsonAsync("api/users/" + userId + "/memberships/" + membershipId, paymentDto);

                return await ApiClientHelper.GenerateActionResult(response);
            }
            catch (Exception ex)
            {
                return ModelActionResult.Fail(GymFaultType.ApiCallFailed, ex.Message);
            }
        }

        public async Task<ClubDetailsDto?> GetClubByIdAsync(int id)
        {
            if (!_authenticatedUser.IsAuthenticated)
                return null;

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
            var response = await _httpClient.GetAsync("api/clubs/" + id);
            response.EnsureSuccessStatusCode();
            var club = await response.Content.ReadFromJsonAsync<ClubDetailsDto>();
            return club;
        }

        public async Task<List<ClubDto>> GetClubsAsync()
        {
            if (!_authenticatedUser.IsAuthenticated)
                return [];

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
            var response = await _httpClient.GetAsync("api/clubs");
            response.EnsureSuccessStatusCode();
            var clubs = await response.Content.ReadFromJsonAsync<List<ClubDto>>();
            return clubs ?? [];
        }

        public async Task<ModelActionResult> CreateClubAsync(CreateClubDto clubCreateDto)
        {
            if (!_authenticatedUser.IsAuthenticated)
                return ModelActionResult.Fail(GymFaultType.UserNotAuthenticated);

            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
                var response = await _httpClient.PostAsJsonAsync("api/clubs", clubCreateDto);

                return await ApiClientHelper.GenerateActionResult(response);
            }
            catch (Exception ex)
            {
                return ModelActionResult.Fail(GymFaultType.ApiCallFailed, ex.Message);
            }
        }

        public async Task<ModelActionResult> UpdateClubAsync(int clubId, UpdateClubDto clubUpdateDto)
        {
            if (!_authenticatedUser.IsAuthenticated)
                return ModelActionResult.Fail(GymFaultType.UserNotAuthenticated);

            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
                var response = await _httpClient.PatchAsJsonAsync("api/clubs/" + clubId, clubUpdateDto);

                return await ApiClientHelper.GenerateActionResult(response);
            }
            catch (Exception ex)
            {
                return ModelActionResult.Fail(GymFaultType.ApiCallFailed, ex.Message);
            }
        }

        public async Task<MembershipPlanDetailsDto?> GetMembershipPlanByIdAsync(int id)
        {
            if (!_authenticatedUser.IsAuthenticated)
                return null;

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
            var response = await _httpClient.GetAsync("api/membership-plans/" + id);
            response.EnsureSuccessStatusCode();
            var membershipPlan = await response.Content.ReadFromJsonAsync<MembershipPlanDetailsDto>();
            return membershipPlan;
        }

        public async Task<List<MembershipPlanDto>> GetMembershipPlansAsync()
        {
            if (!_authenticatedUser.IsAuthenticated)
                return [];

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
            var response = await _httpClient.GetAsync("api/membership-plans");
            response.EnsureSuccessStatusCode();
            var membershipPlans = await response.Content.ReadFromJsonAsync<List<MembershipPlanDto>>();
            return membershipPlans ?? [];
        }

        public async Task<ModelActionResult> CreateMembershipPlanAsync(CreateMembershipPlanDto createMembershipPlanDto)
        {
            if (!_authenticatedUser.IsAuthenticated)
                return ModelActionResult.Fail(GymFaultType.UserNotAuthenticated);

            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
                var response = await _httpClient.PostAsJsonAsync("api/membership-plans", createMembershipPlanDto);

                return await ApiClientHelper.GenerateActionResult(response);
            }
            catch (Exception ex)
            {
                return ModelActionResult.Fail(GymFaultType.ApiCallFailed, ex.Message);
            }
        }
    }
}
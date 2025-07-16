using GymManagement.Application.Interfaces.Controllers.DTOs;
using GymManagement.Application.Interfaces.Services.Clubs;
using GymManagement.Application.Interfaces.Services.CoachingPlans;
using GymManagement.Application.Interfaces.Services.Coachings;
using GymManagement.Application.Interfaces.Services.MembershipPlans;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Shared.Core.AuthManager;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;

namespace GymManagement.Presentation.WebApp.ApiClients
{
    public class GymAdminApiClient(HttpClient _httpClient, AuthenticatedUser _authenticatedUser, IApiClientHelper _apiClientHelper)
    {
        public async Task<ModelActionResult<UserDetailsDto>> MeAsync()
        {
            var result = await _apiClientHelper.CheckAuthenticatedUser();
            if (!result.Success)
                return ModelActionResult<UserDetailsDto>.Fail(result);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);

            try
            {
                var response = await _httpClient.GetAsync("api/app/users/me");
                response.EnsureSuccessStatusCode();

                var user = await response.Content.ReadFromJsonAsync<UserDetailsDto>();
                if (user == null)
                    return ModelActionResult<UserDetailsDto>.Fail(GymFaultType.ApiCallFailed);

                return ModelActionResult<UserDetailsDto>.Ok(user);
            }
            catch (Exception ex)
            {
                return ModelActionResult<UserDetailsDto>.Fail(GymFaultType.ApiCallFailed, ex.Message);
            }
        }

        public async Task<UserDetailsDto?> GetUserByIdAsync(int id)
        {
            var result = await _apiClientHelper.CheckAuthenticatedUser();
            if (!result.Success)
                return null;

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
            var response = await _httpClient.GetAsync("api/admin/users/" + id);
            response.EnsureSuccessStatusCode();
            var user = await response.Content.ReadFromJsonAsync<UserDetailsDto>();
            return user;
        }

        public async Task<List<UserDto>> GetUsersAsync(UserFilter userFilter)
        {
            var result = await _apiClientHelper.CheckAuthenticatedUser();
            if (!result.Success)
                return [];

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
            var response = await _httpClient.GetAsync($"api/admin/users?page-size={userFilter.PageSize}&page-number={userFilter.PageNumber}&email={userFilter.Email}");
            response.EnsureSuccessStatusCode();
            var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();
            return users ?? [];
        }

        public async Task<ModelActionResult> CreateUserAsync(CreateUserDto userCreateDto)
        {
            var result = await _apiClientHelper.CheckAuthenticatedUser();
            if (!result.Success)
                return ModelActionResult.Fail(GymFaultType.UserNotAuthenticated);

            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
                var response = await _httpClient.PostAsJsonAsync("api/admin/users", userCreateDto);

                return await _apiClientHelper.GenerateActionResult(response);
            }
            catch (Exception ex)
            {
                return ModelActionResult.Fail(GymFaultType.ApiCallFailed, ex.Message);
            }
        }

        public async Task<ModelActionResult> UpdateUserAsync(int userId, UpdateUserDto userUpdateDto)
        {
            var result = await _apiClientHelper.CheckAuthenticatedUser();
            if (!result.Success)
                return ModelActionResult.Fail(GymFaultType.UserNotAuthenticated);

            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
                var response = await _httpClient.PatchAsJsonAsync("api/admin/users/" + userId, userUpdateDto);

                return await _apiClientHelper.GenerateActionResult(response);
            }
            catch (Exception ex)
            {
                return ModelActionResult.Fail(GymFaultType.ApiCallFailed, ex.Message);
            }
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var result = await _apiClientHelper.CheckAuthenticatedUser();
            if (!result.Success)
                return false;
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
                var response = await _httpClient.DeleteAsync("api/admin/users/" + userId);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<ModelActionResult> SubscribeUserAsync(int userId, SubscribeUserDto userSubscribeDto)
        {
            var result = await _apiClientHelper.CheckAuthenticatedUser();
            if (!result.Success)
                return ModelActionResult.Fail(GymFaultType.UserNotAuthenticated);

            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
                var response = await _httpClient.PostAsJsonAsync("api/admin/users/" + userId + "/subscribe", userSubscribeDto);

                return await _apiClientHelper.GenerateActionResult(response);
            }
            catch (Exception ex)
            {
                return ModelActionResult.Fail(GymFaultType.ApiCallFailed, ex.Message);
            }
        }

        public async Task<ModelActionResult> PayMembershipAsync(int userId, int membershipId, PaymentDto paymentDto)
        {
            var result = await _apiClientHelper.CheckAuthenticatedUser();
            if (!result.Success)
                return ModelActionResult.Fail(GymFaultType.UserNotAuthenticated);

            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
                var response = await _httpClient.PostAsJsonAsync("api/admin/users/" + userId + "/memberships/" + membershipId, paymentDto);

                return await _apiClientHelper.GenerateActionResult(response);
            }
            catch (Exception ex)
            {
                return ModelActionResult.Fail(GymFaultType.ApiCallFailed, ex.Message);
            }
        }

        public async Task<ClubDetailsDto?> GetClubByIdAsync(int id)
        {
            var result = await _apiClientHelper.CheckAuthenticatedUser();
            if (!result.Success)
                return null;

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
            var response = await _httpClient.GetAsync("api/admin/clubs/" + id);
            response.EnsureSuccessStatusCode();
            var club = await response.Content.ReadFromJsonAsync<ClubDetailsDto>();
            return club;
        }

        public async Task<List<ClubDto>> GetClubsAsync()
        {
            var result = await _apiClientHelper.CheckAuthenticatedUser();
            if (!result.Success)
                return [];

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
            var response = await _httpClient.GetAsync("api/admin/clubs");
            response.EnsureSuccessStatusCode();
            var clubs = await response.Content.ReadFromJsonAsync<List<ClubDto>>();
            return clubs ?? [];
        }

        public async Task<ModelActionResult> CreateClubAsync(CreateClubDto clubCreateDto)
        {
            var result = await _apiClientHelper.CheckAuthenticatedUser();
            if (!result.Success)
                return ModelActionResult.Fail(GymFaultType.UserNotAuthenticated);

            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
                var response = await _httpClient.PostAsJsonAsync("api/admin/clubs", clubCreateDto);

                return await _apiClientHelper.GenerateActionResult(response);
            }
            catch (Exception ex)
            {
                return ModelActionResult.Fail(GymFaultType.ApiCallFailed, ex.Message);
            }
        }

        public async Task<ModelActionResult> UpdateClubAsync(int clubId, UpdateClubDto clubUpdateDto)
        {
            var result = await _apiClientHelper.CheckAuthenticatedUser();
            if (!result.Success)
                return ModelActionResult.Fail(GymFaultType.UserNotAuthenticated);

            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
                var response = await _httpClient.PatchAsJsonAsync("api/admin/clubs/" + clubId, clubUpdateDto);

                return await _apiClientHelper.GenerateActionResult(response);
            }
            catch (Exception ex)
            {
                return ModelActionResult.Fail(GymFaultType.ApiCallFailed, ex.Message);
            }
        }

        public async Task<MembershipPlanDetailsDto?> GetMembershipPlanByIdAsync(int id)
        {
            var result = await _apiClientHelper.CheckAuthenticatedUser();
            if (!result.Success)
                return null;

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
            var response = await _httpClient.GetAsync("api/admin/membership-plans/" + id);
            response.EnsureSuccessStatusCode();
            var membershipPlan = await response.Content.ReadFromJsonAsync<MembershipPlanDetailsDto>();
            return membershipPlan;
        }

        public async Task<List<MembershipPlanDto>> GetMembershipPlansAsync()
        {
            var result = await _apiClientHelper.CheckAuthenticatedUser();
            if (!result.Success)
                return [];

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
            var response = await _httpClient.GetAsync("api/admin/membership-plans");
            response.EnsureSuccessStatusCode();
            var membershipPlans = await response.Content.ReadFromJsonAsync<List<MembershipPlanDto>>();
            return membershipPlans ?? [];
        }

        public async Task<ModelActionResult> CreateMembershipPlanAsync(CreateMembershipPlanDto createMembershipPlanDto)
        {
            var result = await _apiClientHelper.CheckAuthenticatedUser();
            if (!result.Success)
                return ModelActionResult.Fail(GymFaultType.UserNotAuthenticated);

            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
                var response = await _httpClient.PostAsJsonAsync("api/admin/membership-plans", createMembershipPlanDto);

                return await _apiClientHelper.GenerateActionResult(response);
            }
            catch (Exception ex)
            {
                return ModelActionResult.Fail(GymFaultType.ApiCallFailed, ex.Message);
            }
        }

        public async Task<ModelActionResult> UpdateMembershipPlanAsync(int membershipPlanId, UpdateMembershipPlanDto membershipPlanUpdateDto)
        {
            var result = await _apiClientHelper.CheckAuthenticatedUser();
            if (!result.Success)
                return ModelActionResult.Fail(GymFaultType.UserNotAuthenticated);

            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
                var response = await _httpClient.PatchAsJsonAsync("api/admin/membership-plans/" + membershipPlanId, membershipPlanUpdateDto);

                return await _apiClientHelper.GenerateActionResult(response);
            }
            catch (Exception ex)
            {
                return ModelActionResult.Fail(GymFaultType.ApiCallFailed, ex.Message);
            }
        }

        public async Task<List<CoachingPlanDto>> GetCoachingPlansAsync(bool onlyAuthenticatedCoach = false)
        {
            var result = await _apiClientHelper.CheckAuthenticatedUser();
            if (!result.Success)
                return [];

            var path = "api/admin/coaching-plans";
            if (onlyAuthenticatedCoach)
                path += "?coachEmail=" + _authenticatedUser.Email;

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
            var response = await _httpClient.GetAsync(path);
            response.EnsureSuccessStatusCode();
            var coachingPlans = await response.Content.ReadFromJsonAsync<List<CoachingPlanDto>>();
            return coachingPlans ?? [];
        }

        public async Task<CoachingPlanDetailsDto?> GetCoachingPlanByIdAsync(int id)
        {
            var result = await _apiClientHelper.CheckAuthenticatedUser();
            if (!result.Success)
                return null;

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
            var response = await _httpClient.GetAsync("api/admin/coaching-plans/" + id);
            response.EnsureSuccessStatusCode();
            var coachingPlan = await response.Content.ReadFromJsonAsync<CoachingPlanDetailsDto>();
            return coachingPlan;
        }

        public async Task<ModelActionResult> CreateCoachingPlanAsync(CreateCoachingPlanDto coachingPlanCreateDto)
        {
            var result = await _apiClientHelper.CheckAuthenticatedUser();
            if (!result.Success)
                return ModelActionResult.Fail(GymFaultType.UserNotAuthenticated);

            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
                var response = await _httpClient.PostAsJsonAsync("api/admin/coaching-plans", coachingPlanCreateDto);

                return await _apiClientHelper.GenerateActionResult(response);
            }
            catch (Exception ex)
            {
                return ModelActionResult.Fail(GymFaultType.ApiCallFailed, ex.Message);
            }
        }

        public async Task<List<CoachingDto>> GetCoachingsAsync(bool onlyAuthenticatedCoach = false)
        {
            var result = await _apiClientHelper.CheckAuthenticatedUser();
            if (!result.Success)
                return [];

            var meResult = await MeAsync();
            if (!meResult.Success)
                return [];

            var me = meResult.Results;

            var path = "api/admin/coachings";
            if (onlyAuthenticatedCoach)
                path += "?coachId=" + me.Id;

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
            var response = await _httpClient.GetAsync(path);
            response.EnsureSuccessStatusCode();
            var coachingPlans = await response.Content.ReadFromJsonAsync<List<CoachingDto>>();
            return coachingPlans ?? [];
        }

        public async Task<CoachingDetailsDto?> GetCoachingByIdAsync(int id)
        {
            var result = await _apiClientHelper.CheckAuthenticatedUser();
            if (!result.Success)
                return null;

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);
            var response = await _httpClient.GetAsync("api/admin/coachings/" + id);
            response.EnsureSuccessStatusCode();
            var coaching = await response.Content.ReadFromJsonAsync<CoachingDetailsDto>();
            return coaching;
        }
    }
}
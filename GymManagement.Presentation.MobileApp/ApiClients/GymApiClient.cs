using Azure;
using GymManagement.Application.Interfaces.Controllers.DTOs;
using GymManagement.Application.Interfaces.Services.Clubs;
using GymManagement.Application.Interfaces.Services.MembershipPlans;
using GymManagement.Application.Interfaces.Services.Memberships;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Presentation.MobileApp.Services;
using GymManagement.Shared.Core.AuthManager;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;
using System.Net;
using System.Net.Http.Json;

namespace GymManagement.Presentation.MobileApp.ApiClients
{
    public class GymApiClient(HttpClient _httpClient, AuthenticatedUser _authenticatedUser, IApiClientHelper _apiClientHelper)
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

        public async Task<ModelActionResult<UserDetailsDto>> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
        {
            var result = await _apiClientHelper.CheckAuthenticatedUser();
            if (!result.Success)
                return ModelActionResult<UserDetailsDto>.Fail(result);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);

            try
            {
                var response = await _httpClient.PatchAsJsonAsync("api/app/users/" + id, updateUserDto);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var errorDto = await response.Content.ReadFromJsonAsync<ErrorDto>();
                    if (errorDto != null)
                        return ModelActionResult<UserDetailsDto>.Fail((GymFaultType)errorDto.FaultCode, errorDto.Message);

                    return ModelActionResult<UserDetailsDto>.Fail(GymFaultType.ApiCallFailed);
                }

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

        public async Task<ModelActionResult> DeleteUserAsync(int userId)
        {
            var result = await _apiClientHelper.CheckAuthenticatedUser();
            if (!result.Success)
                return ModelActionResult.Fail(result);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);

            try
            {
                var response = await _httpClient.DeleteAsync("api/app/users/" + userId);

                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    var errorDto = await response.Content.ReadFromJsonAsync<ErrorDto>();
                    if (errorDto != null)
                        return ModelActionResult.Fail((GymFaultType)errorDto.FaultCode, errorDto.Message);

                    return ModelActionResult.Fail(GymFaultType.ApiCallFailed);
                }

                return ModelActionResult.Ok;
            }
            catch (Exception ex)
            {
                return ModelActionResult.Fail(GymFaultType.ApiCallFailed, ex.Message);
            }
        }

        public async Task<ModelActionResult<List<MembershipPlanDto>>> GetMembershipPlansAsync()
        {
            var result = await _apiClientHelper.CheckAuthenticatedUser();
            if (!result.Success)
                return ModelActionResult<List<MembershipPlanDto>>.Fail(result);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);

            try
            {
                var response = await _httpClient.GetAsync("api/app/membership-plans");
                response.EnsureSuccessStatusCode();

                var membershipPlans = await response.Content.ReadFromJsonAsync<List<MembershipPlanDto>>();
                if (membershipPlans == null)
                    return ModelActionResult<List<MembershipPlanDto>>.Fail(GymFaultType.ApiCallFailed);

                return ModelActionResult<List<MembershipPlanDto>>.Ok(membershipPlans);
            }
            catch (Exception ex)
            {
                return ModelActionResult<List<MembershipPlanDto>>.Fail(GymFaultType.ApiCallFailed, ex.Message);
            }
        }

        public async Task<ModelActionResult<List<ClubDto>>> GetClubsAsync()
        {
            var result = await _apiClientHelper.CheckAuthenticatedUser();
            if (!result.Success)
                return ModelActionResult<List<ClubDto>>.Fail(result);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);

            try
            {
                var response = await _httpClient.GetAsync("api/app/clubs");
                response.EnsureSuccessStatusCode();

                var clubs = await response.Content.ReadFromJsonAsync<List<ClubDto>>();
                if (clubs == null)
                    return ModelActionResult<List<ClubDto>>.Fail(GymFaultType.ApiCallFailed);

                return ModelActionResult<List<ClubDto>>.Ok(clubs);
            }
            catch (Exception ex)
            {
                return ModelActionResult<List<ClubDto>>.Fail(GymFaultType.ApiCallFailed, ex.Message);
            }
        }

        public async Task<ModelActionResult<UserDetailsDto>> SubscribeUserAsync(SubscriptionFlowData subscriptionFlowData)
        {
            var result = await _apiClientHelper.CheckAuthenticatedUser();
            if (!result.Success)
                return ModelActionResult<UserDetailsDto>.Fail(GymFaultType.UserNotAuthenticated);

            var subscribeUserDto = new SubscribeUserDto
            {
                MembershipPlanId = subscriptionFlowData.MembershipPlan?.Id ?? 0,
                HomeClubId = subscriptionFlowData.HomeClub?.Id ?? 0,
                StartDate = subscriptionFlowData.StartDate,
                RenewWhenExpiry = subscriptionFlowData.RenewWhenExpiry,
                MembershipPeriod = subscriptionFlowData.MembershipPeriod
            };

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/app/users/" + subscriptionFlowData.UserId + "/subscribe", subscribeUserDto);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var errorDto = await response.Content.ReadFromJsonAsync<ErrorDto>();
                    if (errorDto != null)
                        return ModelActionResult<UserDetailsDto>.Fail((GymFaultType)errorDto.FaultCode, errorDto.Message);

                    return ModelActionResult<UserDetailsDto>.Fail(GymFaultType.ApiCallFailed);
                }

                var membershipDetails = await response.Content.ReadFromJsonAsync<UserDetailsDto>();
                if (membershipDetails == null)
                    return ModelActionResult<UserDetailsDto>.Fail(GymFaultType.ApiCallFailed);

                return ModelActionResult<UserDetailsDto>.Ok(membershipDetails);
            }
            catch (Exception ex)
            {
                return ModelActionResult<UserDetailsDto>.Fail(GymFaultType.ApiCallFailed, ex.Message);
            }
        }

        public async Task<ModelActionResult<string>> GetQrCodeSvgAsync()
        {
            var result = await _apiClientHelper.CheckAuthenticatedUser();
            if (!result.Success)
                return ModelActionResult<string>.Fail(GymFaultType.UserNotAuthenticated);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);

            try
            {
                var response = await _httpClient.GetAsync("api/app/users/qr-code");

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var errorDto = await response.Content.ReadFromJsonAsync<ErrorDto>();
                    if (errorDto != null)
                        return ModelActionResult<string>.Fail((GymFaultType)errorDto.FaultCode, errorDto.Message);

                    return ModelActionResult<string>.Fail(GymFaultType.ApiCallFailed);
                }

                var qrCodeSvg = await response.Content.ReadAsStringAsync();
                if (qrCodeSvg == null)
                    return ModelActionResult<string>.Fail(GymFaultType.ApiCallFailed);

                return ModelActionResult<string>.Ok(qrCodeSvg);
            }
            catch (Exception ex)
            {

                return ModelActionResult<string>.Fail(GymFaultType.ApiCallFailed, ex.Message);
            }
        }

        public async Task<ModelActionResult<MembershipDetailsDto>> GetMembershipByIdAsync(int userId, int membershipId)
        {
            var result = await _apiClientHelper.CheckAuthenticatedUser();
            if (!result.Success)
                return ModelActionResult<MembershipDetailsDto>.Fail(GymFaultType.UserNotAuthenticated);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);

            try
            {
                var response = await _httpClient.GetAsync($"api/app/users/{userId}/memberships/{membershipId}");

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var errorDto = await response.Content.ReadFromJsonAsync<ErrorDto>();
                    if (errorDto != null)
                        return ModelActionResult<MembershipDetailsDto>.Fail((GymFaultType)errorDto.FaultCode, errorDto.Message);

                    return ModelActionResult<MembershipDetailsDto>.Fail(GymFaultType.ApiCallFailed);
                }

                var membership = await response.Content.ReadFromJsonAsync<MembershipDetailsDto>();
                if (membership == null)
                    return ModelActionResult<MembershipDetailsDto>.Fail(GymFaultType.ApiCallFailed);

                return ModelActionResult<MembershipDetailsDto>.Ok(membership);
            }
            catch (Exception ex)
            {

                return ModelActionResult<MembershipDetailsDto>.Fail(GymFaultType.ApiCallFailed, ex.Message);
            }
        }

        public async Task<ModelActionResult> PayMembershipAsync(int userId, int membershipId, PaymentDto paymentDto)
        {
            var result = await _apiClientHelper.CheckAuthenticatedUser();
            if (!result.Success)
                return ModelActionResult.Fail(GymFaultType.UserNotAuthenticated);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authenticatedUser.Token);

            try
            {
                var response = await _httpClient.PostAsJsonAsync($"api/app/users/{userId}/memberships/{membershipId}", paymentDto);

                return await _apiClientHelper.GenerateActionResult(response);
            }
            catch (Exception ex)
            {
                return ModelActionResult.Fail(GymFaultType.ApiCallFailed, ex.Message);
            }
        }
    }
}

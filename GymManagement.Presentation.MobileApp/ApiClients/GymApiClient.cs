using GymManagement.Application.Interfaces.Controllers.DTOs;
using GymManagement.Application.Interfaces.Services.Users;
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
                    return ModelActionResult<UserDetailsDto>.Fail(GymFaultType.UserNotFound, "User not found");

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
    }
}

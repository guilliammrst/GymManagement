using GymManagement.Application.Interfaces.Repositories.Users;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Shared.Core.ClaimsHelper;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;
using System.Security.Claims;

namespace GymManagement.Application.Services.Users
{
    public class UserVerificationService(IUserQueryRepository _userQueryRepository) : IUserVerificationService
    {
        public async Task<ModelActionResult> VerifyAuthenticatedUser(int userId, ClaimsPrincipal claimsPrincipal)
        {
            var authenticatedUserEmailResult = claimsPrincipal.GetEmail();
            if (!authenticatedUserEmailResult.Success)
                return ModelActionResult.Fail(authenticatedUserEmailResult);

            var authenticatedUserEmail = authenticatedUserEmailResult.Results;

            var authenticatedUserResult = await _userQueryRepository.GetUserByEmailAsync(authenticatedUserEmail);
            if (!authenticatedUserResult.Success)
                return ModelActionResult.Fail(authenticatedUserResult);

            var authenticatedUser = authenticatedUserResult.Results;

            if (authenticatedUser.Id != userId)
                return ModelActionResult.Fail(GymFaultType.UserNotAuthorized, "You are not authorized to access this resource.");

            return ModelActionResult.Ok;
        }
    }
}

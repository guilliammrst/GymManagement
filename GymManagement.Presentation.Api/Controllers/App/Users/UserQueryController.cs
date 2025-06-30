using GymManagement.Shared.Core.Constants;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Presentation.Api.Controllers.App.Users
{
    [ApiController]
    [Route("api/users")]
    [Authorize(Roles = RoleConstants.None + ", " + RoleConstants.Member + ", " + RoleConstants.Coach + ", " + RoleConstants.Staff + ", " + RoleConstants.Manager)]
    public class UserQueryController(IUserVerificationService _userVerificationService, 
        IUserQueryService _userQueryService) : GymBaseController
    {
        [HttpGet("{userId}")]
        public async Task<ActionResult> GetUserById(int userId)
        {
            var verificationResult = await _userVerificationService.VerifyAuthenticatedUser(userId, User);
            if (!verificationResult.Success)
                return ConvertActionResult(verificationResult);

            var userResult = await _userQueryService.GetUserByIdAsync(userId);

            if (!userResult.Success)
                return ConvertActionResult(userResult);

            var user = userResult.Results;

            return Ok(user);
        }
    }
}

using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Shared.Core.ClaimsPrincipalExt;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Presentation.Api.Controllers.App.Users
{
    [ApiController]
    [Route("api/app/users")]
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

        [HttpGet("me")]
        public async Task<ActionResult> Me()
        {
            var emailResult = User.GetEmail();
            if (!emailResult.Success)
                return ConvertActionResult(emailResult);

            var email = emailResult.Results;

            var userResult = await _userQueryService.MeAsync(email);
            if (!userResult.Success)
                return ConvertActionResult(userResult);

            var user = userResult.Results;

            return Ok(user);
        }
    }
}

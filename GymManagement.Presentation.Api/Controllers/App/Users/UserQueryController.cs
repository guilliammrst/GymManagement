using GymManagement.Shared.Core.Constants;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GymManagement.Presentation.Api.Controllers.App.Users
{
    /// <summary>
    /// Controller for user self-query operations (App access)
    /// </summary>
    [ApiController]
    [Route("api/app/users")]
    [Authorize(Roles = RoleConstants.None + ", " + RoleConstants.Member + ", " + RoleConstants.Coach + ", " + RoleConstants.Staff + ", " + RoleConstants.Manager)]
    [SwaggerTag("User Self-Queries - App")]
    public class UserQueryController(IUserVerificationService _userVerificationService, 
        IUserQueryService _userQueryService) : GymBaseController
    {
        /// <summary>
        /// Retrieves the authenticated user's own profile information
        /// </summary>
        /// <param name="userId">The ID of the user (must match authenticated user)</param>
        /// <returns>User profile information</returns>
        /// <response code="200">User profile retrieved successfully</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="403">Cannot access another user's profile</response>
        /// <response code="404">User not found</response>
        [HttpGet("{userId}")]
        [SwaggerOperation(Summary = "Get own user profile", Description = "Allows a user to retrieve their own profile information")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

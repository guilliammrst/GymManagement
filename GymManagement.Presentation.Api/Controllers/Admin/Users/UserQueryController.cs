using GymManagement.Shared.Core.Constants;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GymManagement.Application.Interfaces.Controllers.DTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace GymManagement.Presentation.Api.Controllers.Admin.Users
{
    /// <summary>
    /// Controller for querying user information (Admin access required)
    /// </summary>
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Roles = RoleConstants.Staff + ", " + RoleConstants.Manager)]
    [SwaggerTag("User Queries - Admin")]
    public class UserQueryController(IUserQueryService _userQueryService) : GymBaseController
    {
        /// <summary>
        /// Retrieves a specific user by their ID
        /// </summary>
        /// <param name="userId">The unique identifier of the user</param>
        /// <returns>Detailed user information</returns>
        /// <response code="200">User found and returned successfully</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="404">User not found</response>
        [HttpGet("{userId}")]
        [SwaggerOperation(Summary = "Get user by ID", Description = "Retrieves detailed information about a specific user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetUserById(int userId)
        {
            var userResult = await _userQueryService.GetUserByIdAsync(userId);

            if (!userResult.Success)
                return ConvertActionResult(userResult);

            var user = userResult.Results;

            return Ok(user);
        }

        /// <summary>
        /// Retrieves a paginated list of users with optional filtering
        /// </summary>
        /// <param name="filter">Filter criteria for user search</param>
        /// <returns>List of users matching the filter criteria</returns>
        /// <response code="200">Users retrieved successfully</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="400">Invalid filter parameters</response>
        [HttpGet]
        [SwaggerOperation(Summary = "Get users with filters", Description = "Retrieves a paginated list of users with optional email filtering")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetUsers([FromQuery] UserFilter filter)
        {
            var usersResult = await _userQueryService.GetUsersAsync(new UserServiceFilter
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                Email = filter.Email
            });

            if (!usersResult.Success)
                return ConvertActionResult(usersResult);

            var users = usersResult.Results;

            return Ok(users);
        }
    }
}

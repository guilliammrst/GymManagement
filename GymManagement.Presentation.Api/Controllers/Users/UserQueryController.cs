using GymManagement.Shared.Core.Constants;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GymManagement.Application.Interfaces.Controllers.DTOs;

namespace GymManagement.Presentation.Api.Controllers.Users
{
    [ApiController]
    [Route("api/users")]
    [Authorize(Roles = RoleConstants.Staff + ", " + RoleConstants.Manager)]
    public class UserQueryController(IUserQueryService _userQueryService) : GymBaseController
    {
        [HttpGet("{userId}")]
        public async Task<ActionResult> GetUserById(int userId)
        {
            var userResult = await _userQueryService.GetUserByIdAsync(userId);

            if (!userResult.Success)
                return ConvertActionResult(userResult);

            var user = userResult.Results;

            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult> GetAccounts([FromQuery] UserFilter filter)
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

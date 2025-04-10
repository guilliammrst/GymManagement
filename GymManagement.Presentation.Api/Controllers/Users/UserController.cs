using GymManagement.Shared.Core.Constants;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GymManagement.Presentation.Api.DTOs;

namespace GymManagement.Presentation.Api.Controllers.Users
{
    [ApiController]
    [Route("api/users")]
    [Authorize(Roles = RoleConstants.Staff + ", " + RoleConstants.Manager)]
    public class UserController(IUserService _userService) : GymBaseController
    {
        [HttpGet("{userId}")]
        public async Task<ActionResult> GetUserById(int userId)
        {
            var userResult = await _userService.GetUserById(userId);

            if (!userResult.Success)
                return ConvertActionResult(userResult);

            var user = userResult.Results;

            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult> GetAccounts()
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == ClaimsTypes.Email)?.Value;

            var usersResult = await _userService.GetUsers();

            if (!usersResult.Success)
                return ConvertActionResult(usersResult);

            var users = usersResult.Results;

            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            var userResult = await _userService.CreateUser(new UserCreateDto
            {
                Name = userDto.Name,
                Surname = userDto.Surname,
                Birthdate = userDto.Birthdate,
                Password = userDto.Password,
                Role = userDto.Role,
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber,
                Gender = userDto.Gender
            });

            if (!userResult.Success)
                return ConvertActionResult(userResult);
            
            var user = userResult.Results;

            return new ObjectResult(user)
            {
                StatusCode = StatusCodes.Status201Created
            };
        }
    }
}

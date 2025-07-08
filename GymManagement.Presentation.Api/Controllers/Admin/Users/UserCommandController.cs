using GymManagement.Application.Interfaces.Controllers.DTOs;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Presentation.Api.Controllers.Admin.Users
{
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Roles = RoleConstants.Staff + ", " + RoleConstants.Manager)]
    public class UserCommandController(IUserCommandService _userCommandService) : GymBaseController
    {
        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            var userResult = await _userCommandService.CreateUserAsync(new UserCreateDto
            {
                Name = userDto.Name,
                Surname = userDto.Surname,
                Birthdate = userDto.Birthdate,
                Password = userDto.Password,
                Role = userDto.Role,
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber,
                Gender = userDto.Gender,
                Country = userDto.Country,
                City = userDto.City,
                Street = userDto.Street,
                PostalCode = userDto.PostalCode,
                Number = userDto.Number
            });

            if (!userResult.Success)
                return ConvertActionResult(userResult);

            var user = userResult.Results;

            return new ObjectResult(user)
            {
                StatusCode = StatusCodes.Status201Created
            };
        }

        [HttpPatch("{userId}")]
        public async Task<ActionResult> UpdateUser([FromRoute] int userId, [FromBody] UpdateUserDto userDto)
        {

            var userResult = await _userCommandService.UpdateUserAsync(new UserUpdateDto
            {
                Id = userId,
                Name = userDto.Name,
                Surname = userDto.Surname,
                Birthdate = userDto.Birthdate,
                Password = userDto.Password,
                Role = userDto.Role,
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber,
                Gender = userDto.Gender,
                Country = userDto.Country,
                City = userDto.City,
                Street = userDto.Street,
                PostalCode = userDto.PostalCode,
                Number = userDto.Number
            });

            if (!userResult.Success)
                return ConvertActionResult(userResult);

            var user = userResult.Results;

            return Ok(user);
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteUser([FromRoute] int userId)
        {
            var userResult = await _userCommandService.DeleteUserAsync(userId);
            if (!userResult.Success)
                return ConvertActionResult(userResult);

            return NoContent();
        }
    }
}

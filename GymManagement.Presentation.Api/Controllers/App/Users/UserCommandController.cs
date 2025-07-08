using GymManagement.Application.Interfaces.Controllers.DTOs;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Presentation.Api.Controllers.App.Users
{
    [ApiController]
    [Route("api/app/users")]
    [Authorize(Roles = RoleConstants.None + ", " + RoleConstants.Member + ", " + RoleConstants.Coach + ", " + RoleConstants.Staff + ", " + RoleConstants.Manager)]
    public class UserCommandController(IUserVerificationService _userVerificationService,
        IUserCommandService _userCommandService) : GymBaseController
    {
        [HttpPatch("{userId}")]
        public async Task<ActionResult> UpdateUser([FromRoute] int userId, [FromBody] UpdateUserDto userDto)
        {
            var verificationResult = await _userVerificationService.VerifyAuthenticatedUser(userId, User);
            if (!verificationResult.Success)
                return ConvertActionResult(verificationResult);

            var userResult = await _userCommandService.UpdateUserAsync(new UserUpdateDto
            {
                Id = userId,
                Name = userDto.Name,
                Surname = userDto.Surname,
                Birthdate = userDto.Birthdate,
                Password = userDto.Password,
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
            var verificationResult = await _userVerificationService.VerifyAuthenticatedUser(userId, User);
            if (!verificationResult.Success)
                return ConvertActionResult(verificationResult);

            var userResult = await _userCommandService.DeleteUserAsync(userId);
            if (!userResult.Success)
                return ConvertActionResult(userResult);

            return NoContent();
        }
    }
}

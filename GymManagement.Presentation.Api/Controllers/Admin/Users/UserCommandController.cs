using GymManagement.Application.Interfaces.Controllers.DTOs;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GymManagement.Presentation.Api.Controllers.Admin.Users
{
    /// <summary>
    /// Controller for managing user operations (Admin access required)
    /// </summary>
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Roles = RoleConstants.Staff + ", " + RoleConstants.Manager)]
    [SwaggerTag("User Management - Admin")]
    public class UserCommandController(IUserCommandService _userCommandService) : GymBaseController
    {
        /// <summary>
        /// Creates a new user in the system
        /// </summary>
        /// <param name="userDto">User creation data</param>
        /// <returns>Created user information</returns>
        /// <response code="201">User created successfully</response>
        /// <response code="400">Invalid user data provided</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="409">User with this email already exists</response>
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new user", Description = "Creates a new user with the provided information")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
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

        /// <summary>
        /// Updates an existing user
        /// </summary>
        /// <param name="userId">The ID of the user to update</param>
        /// <param name="userDto">Updated user data</param>
        /// <returns>Updated user information</returns>
        /// <response code="200">User updated successfully</response>
        /// <response code="400">Invalid user data provided</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="404">User not found</response>
        [HttpPatch("{userId}")]
        [SwaggerOperation(Summary = "Update an existing user", Description = "Updates user information with the provided data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Deletes a user from the system
        /// </summary>
        /// <param name="userId">The ID of the user to delete</param>
        /// <returns>No content on successful deletion</returns>
        /// <response code="204">User deleted successfully</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="404">User not found</response>
        [HttpDelete("{userId}")]
        [SwaggerOperation(Summary = "Delete a user", Description = "Permanently removes a user from the system")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteUser([FromRoute] int userId)
        {
            var userResult = await _userCommandService.DeleteUserAsync(userId);
            if (!userResult.Success)
                return ConvertActionResult(userResult);

            return NoContent();
        }
    }
}

using GymManagement.Application.Interfaces.Controllers.DTOs;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GymManagement.Presentation.Api.Controllers.App.Users
{
    /// <summary>
    /// Controller for user self-management operations (App access)
    /// </summary>
    [ApiController]
    [Route("api/app/users")]
    [Authorize(Roles = RoleConstants.None + ", " + RoleConstants.Member + ", " + RoleConstants.Coach + ", " + RoleConstants.Staff + ", " + RoleConstants.Manager)]
    [SwaggerTag("User Self-Management - App")]
    public class UserCommandController(IUserVerificationService _userVerificationService,
        IUserCommandService _userCommandService) : GymBaseController
    {
        /// <summary>
        /// Updates the authenticated user's own profile information
        /// </summary>
        /// <param name="userId">The ID of the user (must match authenticated user)</param>
        /// <param name="userDto">Updated user profile data</param>
        /// <returns>Updated user information</returns>
        /// <response code="200">User profile updated successfully</response>
        /// <response code="400">Invalid user data provided</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="403">Cannot update another user's profile</response>
        /// <response code="404">User not found</response>
        [HttpPatch("{userId}")]
        [SwaggerOperation(Summary = "Update own user profile", Description = "Allows a user to update their own profile information")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Deletes the authenticated user's own account
        /// </summary>
        /// <param name="userId">The ID of the user (must match authenticated user)</param>
        /// <returns>No content on successful deletion</returns>
        /// <response code="204">User account deleted successfully</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="403">Cannot delete another user's account</response>
        /// <response code="404">User not found</response>
        [HttpDelete("{userId}")]
        [SwaggerOperation(Summary = "Delete own user account", Description = "Allows a user to delete their own account")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

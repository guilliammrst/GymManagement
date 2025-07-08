using GymManagement.Application.Interfaces.Controllers.DTOs;
using GymManagement.Application.Interfaces.Services.Clubs;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GymManagement.Presentation.Api.Controllers.Admin.Clubs
{
    /// <summary>
    /// Controller for managing club operations (Admin access required)
    /// </summary>
    [ApiController]
    [Route("api/admin/clubs")]
    [Authorize(Roles = RoleConstants.Staff + ", " + RoleConstants.Manager)]
    [SwaggerTag("Club Management - Admin")]
    public class ClubCommandController(IClubCommandService _clubCommandService) : GymBaseController
    {
        /// <summary>
        /// Creates a new club in the system
        /// </summary>
        /// <param name="clubDto">Club creation data including name, address, and manager</param>
        /// <returns>Created club information</returns>
        /// <response code="201">Club created successfully</response>
        /// <response code="400">Invalid club data provided</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="409">Club with this name already exists in the location</response>
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new club", Description = "Creates a new club with the provided information")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> CreateClub([FromBody] CreateClubDto clubDto)
        {
            var clubResult = await _clubCommandService.CreateClubAsync(new ClubCreateDto
            {
                Name = clubDto.Name,
                Country = clubDto.Country,
                City = clubDto.City,
                Street = clubDto.Street,
                PostalCode = clubDto.PostalCode,
                Number = clubDto.Number,
                ManagerId = clubDto.ManagerId
            });
            if (!clubResult.Success)
                return ConvertActionResult(clubResult);

            var club = clubResult.Results;

            return new ObjectResult(club)
            {
                StatusCode = StatusCodes.Status201Created
            };
        }

        /// <summary>
        /// Updates an existing club's information
        /// </summary>
        /// <param name="clubId">The ID of the club to update</param>
        /// <param name="clubDto">Updated club data</param>
        /// <returns>No content on successful update</returns>
        /// <response code="204">Club updated successfully</response>
        /// <response code="400">Invalid club data provided</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="404">Club not found</response>
        [HttpPatch("{clubId}")]
        [SwaggerOperation(Summary = "Update an existing club", Description = "Updates club information with the provided data")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateClub([FromRoute] int clubId, [FromBody] UpdateClubDto clubDto)
        {
            var clubResult = await _clubCommandService.UpdateClubAsync(new ClubUpdateDto
            {
                Id = clubId,
                Name = clubDto.Name,
                Country = clubDto.Country,
                City = clubDto.City,
                Street = clubDto.Street,
                PostalCode = clubDto.PostalCode,
                Number = clubDto.Number,
                ManagerId = clubDto.ManagerId
            });
            if (!clubResult.Success)
                return ConvertActionResult(clubResult);

            return NoContent();
        }
    }
}
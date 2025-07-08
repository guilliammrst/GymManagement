using GymManagement.Application.Interfaces.Services.Clubs;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GymManagement.Presentation.Api.Controllers.Admin.Clubs
{
    /// <summary>
    /// Controller for querying club information (Admin access required)
    /// </summary>
    [ApiController]
    [Route("api/admin/clubs")]
    [Authorize(Roles = RoleConstants.Staff + ", " + RoleConstants.Manager)]
    [SwaggerTag("Club Queries - Admin")]
    public class ClubQueryController(IClubQueryService _clubQueryService) : GymBaseController
    {
        /// <summary>
        /// Retrieves a specific club by its ID
        /// </summary>
        /// <param name="clubId">The unique identifier of the club</param>
        /// <returns>Detailed club information</returns>
        /// <response code="200">Club found and returned successfully</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="404">Club not found</response>
        [HttpGet("{clubId}")]
        [SwaggerOperation(Summary = "Get club by ID", Description = "Retrieves detailed information about a specific club")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetClubById(int clubId)
        {
            var clubResult = await _clubQueryService.GetClubByIdAsync(clubId);

            if (!clubResult.Success)
                return ConvertActionResult(clubResult);

            var club = clubResult.Results;

            return Ok(club);
        }

        /// <summary>
        /// Retrieves all clubs in the system
        /// </summary>
        /// <returns>List of all clubs</returns>
        /// <response code="200">Clubs retrieved successfully</response>
        /// <response code="401">Unauthorized access</response>
        [HttpGet]
        [SwaggerOperation(Summary = "Get all clubs", Description = "Retrieves a list of all clubs in the system")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetClubs()
        {
            var clubsResult = await _clubQueryService.GetClubsAsync();

            if (!clubsResult.Success)
                return ConvertActionResult(clubsResult);

            var clubs = clubsResult.Results;

            return Ok(clubs);
        }
    }
}

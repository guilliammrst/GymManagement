using GymManagement.Application.Interfaces.Services.Clubs;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GymManagement.Presentation.Api.Controllers.App.Clubs
{
    /// <summary>
    /// Controller for club queries (App access - read-only)
    /// </summary>
    [ApiController]
    [Route("api/app/clubs")]
    [Authorize(Roles = RoleConstants.None + ", " + RoleConstants.Member + ", " + RoleConstants.Coach + ", " + RoleConstants.Staff + ", " + RoleConstants.Manager)]
    [SwaggerTag("Club Queries - App")]
    public class ClubQueryController(IClubQueryService _clubQueryService) : GymBaseController
    {
        /// <summary>
        /// Retrieves all available clubs for app users
        /// </summary>
        /// <returns>List of all available clubs</returns>
        /// <response code="200">Clubs retrieved successfully</response>
        /// <response code="401">Unauthorized access</response>
        [HttpGet]
        [SwaggerOperation(Summary = "Get all clubs", Description = "Retrieves a list of all available clubs for app users")]
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

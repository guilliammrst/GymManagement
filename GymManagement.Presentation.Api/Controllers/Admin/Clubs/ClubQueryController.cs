using GymManagement.Application.Interfaces.Services.Clubs;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Presentation.Api.Controllers.Admin.Clubs
{
    [ApiController]
    [Route("api/admin/clubs")]
    [Authorize(Roles = RoleConstants.Coach + ", " + RoleConstants.Staff + ", " + RoleConstants.Manager)]
    public class ClubQueryController(IClubQueryService _clubQueryService) : GymBaseController
    {
        [HttpGet("{clubId}")]
        public async Task<ActionResult> GetClubById(int clubId)
        {
            var clubResult = await _clubQueryService.GetClubByIdAsync(clubId);

            if (!clubResult.Success)
                return ConvertActionResult(clubResult);

            var club = clubResult.Results;

            return Ok(club);
        }

        [HttpGet]
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

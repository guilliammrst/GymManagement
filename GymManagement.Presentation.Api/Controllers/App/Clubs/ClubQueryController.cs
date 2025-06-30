using GymManagement.Application.Interfaces.Services.Clubs;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Presentation.Api.Controllers.App.Clubs
{
    [ApiController]
    [Route("api/clubs")]
    [Authorize(Roles = RoleConstants.None + ", " + RoleConstants.Member + ", " + RoleConstants.Coach + ", " + RoleConstants.Staff + ", " + RoleConstants.Manager)]
    public class ClubQueryController(IClubQueryService _clubQueryService) : GymBaseController
    {
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

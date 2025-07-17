using GymManagement.Application.Interfaces.Controllers.DTOs;
using GymManagement.Application.Interfaces.Services.CoachingPlans;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Presentation.Api.Controllers.App.CoachingPlans
{
    [ApiController]
    [Route("api/app/coaching-plans")]
    [Authorize(Roles = RoleConstants.None + ", " + RoleConstants.Member + ", " + RoleConstants.Coach + ", " + RoleConstants.Staff + ", " + RoleConstants.Manager)]
    public class CoachingPlanQueryController(ICoachingPlanQueryService _coachingPlanQueryService) : GymBaseController
    {
        [HttpGet]
        public async Task<ActionResult> GetCoachingPlans([FromQuery] CoachingPlanFilter filter)
        {
            var coachingPlansResult = await _coachingPlanQueryService.GetCoachingPlansAsync(new CoachingPlanServiceFilter
            {
                ClubId = filter.ClubId
            });
            if (!coachingPlansResult.Success)
                return ConvertActionResult(coachingPlansResult);

            var coachingPlans = coachingPlansResult.Results;

            return Ok(coachingPlans);
        }
    }
}

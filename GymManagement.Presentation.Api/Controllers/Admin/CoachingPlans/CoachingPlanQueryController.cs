using GymManagement.Application.Interfaces.Controllers.DTOs;
using GymManagement.Application.Interfaces.Services.CoachingPlans;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Presentation.Api.Controllers.Admin.CoachingPlans
{
    [ApiController]
    [Route("api/admin/coaching-plans")]
    [Authorize(Roles = RoleConstants.Coach + ", " + RoleConstants.Staff + ", " + RoleConstants.Manager)]
    public class CoachingPlanQueryController(ICoachingPlanQueryService _coachingPlanQueryService) : GymBaseController
    {
        [HttpGet("{coachingPlanId}")]
        public async Task<ActionResult> GetCoachingPlanById(int coachingPlanId)
        {
            var coachingPlanResult = await _coachingPlanQueryService.GetCoachingPlanByIdAsync(coachingPlanId);
            if (!coachingPlanResult.Success)
                return ConvertActionResult(coachingPlanResult);

            var coachingPlan = coachingPlanResult.Results;

            return Ok(coachingPlan);
        }

        [HttpGet]
        public async Task<ActionResult> GetCoachingPlans([FromQuery] CoachingPlanFilter filter)
        {
            var coachingPlansResult = await _coachingPlanQueryService.GetCoachingPlansAsync(new CoachingPlanServiceFilter
            {
                ClubId = filter.ClubId,
                CoachEmail = filter.CoachEmail
            });
            if (!coachingPlansResult.Success)
                return ConvertActionResult(coachingPlansResult);

            var coachingPlans = coachingPlansResult.Results;

            return Ok(coachingPlans);
        }
    }
}

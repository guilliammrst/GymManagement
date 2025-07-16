using GymManagement.Application.Interfaces.Controllers.DTOs;
using GymManagement.Application.Interfaces.Services.Coachings;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Presentation.Api.Controllers.Admin.Coachings
{
    [ApiController]
    [Route("api/admin/coachings")]
    [Authorize(Roles = RoleConstants.Coach + ", " + RoleConstants.Staff + ", " + RoleConstants.Manager)]
    public class CoachingQueryController(ICoachingQueryService _coachingQueryService) : GymBaseController
    {
        [HttpGet("{coachingId}")]
        public async Task<ActionResult> GetCoachingPlanById(int coachingId)
        {
            var coachingResult = await _coachingQueryService.GetCoachingByIdAsync(coachingId);
            if (!coachingResult.Success)
                return ConvertActionResult(coachingResult);

            var coaching = coachingResult.Results;

            return Ok(coaching);
        }

        [HttpGet]
        public async Task<ActionResult> GetCoachingPlans([FromQuery] CoachingFilter filter)
        {
            var coachingsResult = await _coachingQueryService.GetCoachingsAsync(new CoachingServiceFilter
            {
                CoachId = filter.CoachId,
                MemberId = filter.MemberId
            });
            if (!coachingsResult.Success)
                return ConvertActionResult(coachingsResult);

            var coachings = coachingsResult.Results;

            return Ok(coachings);
        }
    }
}

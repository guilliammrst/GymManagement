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
    public class CoachingPlanCommandController(ICoachingPlanCommandService _coachingPlanCommandService) : GymBaseController
    {
        [HttpPost]
        public async Task<ActionResult> CreateCoachingPlan([FromBody] CreateCoachingPlanDto createCoachingPlanDto)
        {
            var coachingPlanResult = await _coachingPlanCommandService.CreateCoachingPlanAsync(new CoachingPlanCreateDto
            {
                Description = createCoachingPlanDto.Description,
                CoachId = createCoachingPlanDto.CoachId,
                ClubId = createCoachingPlanDto.ClubId,
                Price = createCoachingPlanDto.Price
            });
            if (!coachingPlanResult.Success)
                return ConvertActionResult(coachingPlanResult);

            var coachingPlan = coachingPlanResult.Results;

            return new ObjectResult(coachingPlan)
            {
                StatusCode = StatusCodes.Status201Created
            };
        }
    }
}

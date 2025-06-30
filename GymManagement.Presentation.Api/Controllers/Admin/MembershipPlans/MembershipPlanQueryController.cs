using GymManagement.Application.Interfaces.Services.MembershipPlans;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Presentation.Api.Controllers.Admin.MembershipPlans
{
    [ApiController]
    [Route("api/admin²/membership-plans")]
    [Authorize(Roles = RoleConstants.Staff + ", " + RoleConstants.Manager)]
    public class MembershipPlanQueryController(IMembershipPlanQueryService _membershipPlanQueryService) : GymBaseController
    {
        [HttpGet("{membershipPlanId}")]
        public async Task<ActionResult> GetMembershipPlanById(int membershipPlanId)
        {
            var membershipPlanResult = await _membershipPlanQueryService.GetMembershipPlanByIdAsync(membershipPlanId);
            if (!membershipPlanResult.Success)
                return ConvertActionResult(membershipPlanResult);

            var membershipPlan = membershipPlanResult.Results;

            return Ok(membershipPlan);
        }

        [HttpGet]
        public async Task<ActionResult> GetMembershipPlans()
        {
            var membershipPlansResult = await _membershipPlanQueryService.GetMembershipPlansAsync();
            if (!membershipPlansResult.Success)
                return ConvertActionResult(membershipPlansResult);

            var membershipPlans = membershipPlansResult.Results;

            return Ok(membershipPlans);
        }
    }
}

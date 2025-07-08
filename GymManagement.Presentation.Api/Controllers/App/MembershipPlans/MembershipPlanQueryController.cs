using GymManagement.Application.Interfaces.Services.MembershipPlans;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Presentation.Api.Controllers.App.MembershipPlans
{
    [ApiController]
    [Route("api/app/membership-plans")]
    [Authorize(Roles = RoleConstants.None + ", " + RoleConstants.Member + ", " + RoleConstants.Coach + ", " + RoleConstants.Staff + ", " + RoleConstants.Manager)]
    public class MembershipPlanQueryController(IMembershipPlanQueryService _membershipPlanQueryService) : GymBaseController
    {
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

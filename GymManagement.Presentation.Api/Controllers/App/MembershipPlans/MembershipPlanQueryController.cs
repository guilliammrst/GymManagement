using GymManagement.Application.Interfaces.Services.MembershipPlans;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GymManagement.Presentation.Api.Controllers.App.MembershipPlans
{
    /// <summary>
    /// Controller for membership plan queries (App access - read-only)
    /// </summary>
    [ApiController]
    [Route("api/app/membership-plans")]
    [Authorize(Roles = RoleConstants.None + ", " + RoleConstants.Member + ", " + RoleConstants.Coach + ", " + RoleConstants.Staff + ", " + RoleConstants.Manager)]
    [SwaggerTag("Membership Plan Queries - App")]
    public class MembershipPlanQueryController(IMembershipPlanQueryService _membershipPlanQueryService) : GymBaseController
    {
        /// <summary>
        /// Retrieves all available membership plans for app users
        /// </summary>
        /// <returns>List of all available membership plans</returns>
        /// <response code="200">Membership plans retrieved successfully</response>
        /// <response code="401">Unauthorized access</response>
        [HttpGet]
        [SwaggerOperation(Summary = "Get all membership plans", Description = "Retrieves a list of all available membership plans for app users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

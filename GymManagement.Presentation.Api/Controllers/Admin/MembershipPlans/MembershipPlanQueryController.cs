using GymManagement.Application.Interfaces.Services.MembershipPlans;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GymManagement.Presentation.Api.Controllers.Admin.MembershipPlans
{
    /// <summary>
    /// Controller for querying membership plan information (Admin access required)
    /// </summary>
    [ApiController]
    [Route("api/admin/membership-plans")]
    [Authorize(Roles = RoleConstants.Staff + ", " + RoleConstants.Manager)]
    [SwaggerTag("Membership Plan Queries - Admin")]
    public class MembershipPlanQueryController(IMembershipPlanQueryService _membershipPlanQueryService) : GymBaseController
    {
        /// <summary>
        /// Retrieves a specific membership plan by its ID
        /// </summary>
        /// <param name="membershipPlanId">The unique identifier of the membership plan</param>
        /// <returns>Detailed membership plan information</returns>
        /// <response code="200">Membership plan found and returned successfully</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="404">Membership plan not found</response>
        [HttpGet("{membershipPlanId}")]
        [SwaggerOperation(Summary = "Get membership plan by ID", Description = "Retrieves detailed information about a specific membership plan")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetMembershipPlanById(int membershipPlanId)
        {
            var membershipPlanResult = await _membershipPlanQueryService.GetMembershipPlanByIdAsync(membershipPlanId);
            if (!membershipPlanResult.Success)
                return ConvertActionResult(membershipPlanResult);

            var membershipPlan = membershipPlanResult.Results;

            return Ok(membershipPlan);
        }

        /// <summary>
        /// Retrieves all membership plans in the system
        /// </summary>
        /// <returns>List of all membership plans</returns>
        /// <response code="200">Membership plans retrieved successfully</response>
        /// <response code="401">Unauthorized access</response>
        [HttpGet]
        [SwaggerOperation(Summary = "Get all membership plans", Description = "Retrieves a list of all membership plans with their pricing and features")]
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

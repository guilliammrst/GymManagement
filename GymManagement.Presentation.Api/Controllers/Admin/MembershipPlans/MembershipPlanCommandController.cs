using GymManagement.Application.Interfaces.Controllers.DTOs;
using GymManagement.Application.Interfaces.Services.MembershipPlans;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GymManagement.Presentation.Api.Controllers.Admin.MembershipPlans
{
    /// <summary>
    /// Controller for managing membership plan operations (Admin access required)
    /// </summary>
    [ApiController]
    [Route("api/admin/membership-plans")]
    [Authorize(Roles = RoleConstants.Staff + ", " + RoleConstants.Manager)]
    [SwaggerTag("Membership Plan Management - Admin")]
    public class MembershipPlanCommandController(IMembershipPlanCommandService _membershipPlanCommandService) : GymBaseController
    {
        /// <summary>
        /// Creates a new membership plan in the system
        /// </summary>
        /// <param name="membershipPlanDto">Membership plan creation data including pricing and features</param>
        /// <returns>Created membership plan information</returns>
        /// <response code="201">Membership plan created successfully</response>
        /// <response code="400">Invalid membership plan data provided</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="409">Membership plan with this description already exists</response>
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new membership plan", Description = "Creates a new membership plan with the provided pricing and features")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> CreateMembershipPlan([FromBody] CreateMembershipPlanDto membershipPlanDto)
        {
            var membershipPlanResult = await _membershipPlanCommandService.CreateMembershipPlanAsync(new MembershipPlanCreateDto
            {
                Description = membershipPlanDto.Description,
                BasePrice = membershipPlanDto.BasePrice,
                MembershipPlanType = membershipPlanDto.MembershipPlanType,
                YearlyDiscount = membershipPlanDto.YearlyDiscount,
                RegistrationFees = membershipPlanDto.RegistrationFees
            });
            if (!membershipPlanResult.Success)
                return ConvertActionResult(membershipPlanResult);

            var membershipPlan = membershipPlanResult.Results;

            return new ObjectResult(membershipPlan)
            {
                StatusCode = StatusCodes.Status201Created
            };
        }

        /// <summary>
        /// Updates an existing membership plan's information
        /// </summary>
        /// <param name="membershipPlanId">The ID of the membership plan to update</param>
        /// <param name="membershipPlanDto">Updated membership plan data</param>
        /// <returns>No content on successful update</returns>
        /// <response code="204">Membership plan updated successfully</response>
        /// <response code="400">Invalid membership plan data provided</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="404">Membership plan not found</response>
        [HttpPatch("{membershipPlanId}")]
        [SwaggerOperation(Summary = "Update an existing membership plan", Description = "Updates membership plan information with the provided data")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateMembershipPlan([FromRoute] int membershipPlanId, [FromBody] UpdateMembershipPlanDto membershipPlanDto)
        {
            var membershipPlanResult = await _membershipPlanCommandService.UpdateMembershipPlanAsync(new MembershipPlanUpdateDto
            {
                Id = membershipPlanId,
                Description = membershipPlanDto.Description,
                BasePrice = membershipPlanDto.BasePrice,
                MembershipPlanType = membershipPlanDto.MembershipPlanType,
                YearlyDiscount = membershipPlanDto.YearlyDiscount,
                RegistrationFees = membershipPlanDto.RegistrationFees,
                IsValid = membershipPlanDto.IsValid
            });
            if (!membershipPlanResult.Success)
                return ConvertActionResult(membershipPlanResult);

            return NoContent();
        }
    }
}

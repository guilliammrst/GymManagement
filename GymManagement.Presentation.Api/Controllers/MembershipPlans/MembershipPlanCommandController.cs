﻿using GymManagement.Application.Interfaces.Services.MembershipPlans;
using GymManagement.Presentation.Api.DTOs;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Presentation.Api.Controllers.MembershipPlans
{
    [ApiController]
    [Route("api/membership-plans")]
    [Authorize(Roles = RoleConstants.Staff + ", " + RoleConstants.Manager)]
    public class MembershipPlanCommandController(IMembershipPlanCommandService _membershipPlanCommandService) : GymBaseController
    {
        [HttpPost]
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
    }
}

﻿using GymManagement.Application.Interfaces.Controllers.DTOs;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Presentation.Api.Controllers.Admin.Users
{
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Roles = RoleConstants.Staff + ", " + RoleConstants.Manager)]
    public class UserSubscriptionController(IUserSubscriptionService _userSubscriptionService) : GymBaseController
    {
        [HttpPost("{userId}/subscribe")]
        public async Task<ActionResult> SubscribeUser(int userId, [FromBody] SubscribeUserDto subscribeUserDto)
        {
            var subscriptionResult = await _userSubscriptionService.AddUserSubscriptionAsync(new UserSubscribeDto
            {
                UserId = userId,
                MembershipPlanId = subscribeUserDto.MembershipPlanId,
                StartDate = subscribeUserDto.StartDate,
                RenewWhenExpiry = subscribeUserDto.RenewWhenExpiry,
                HomeClubId = subscribeUserDto.HomeClubId,
                MembershipPeriod = subscribeUserDto.MembershipPeriod
            });
            if (!subscriptionResult.Success)
                return ConvertActionResult(subscriptionResult);

            var subscription = subscriptionResult.Results;

            return Ok(subscription);
        }

        [HttpPost("{userId}/memberships/{membershipId}")]
        public async Task<ActionResult> PaySubscription(int userId, int membershipId, [FromBody] PaymentDto paymentDto)
        {
            var paymentResult = await _userSubscriptionService.PayUserSubscriptionAsync(new UserPaymentDto
            {
                UserId = userId,
                EntityId = membershipId,
                PaymentMethod = paymentDto.PaymentMethod
            });
            if (!paymentResult.Success)
                return ConvertActionResult(paymentResult);

            var membershipDetails = paymentResult.Results;

            return Ok(membershipDetails);
        }
    }
}

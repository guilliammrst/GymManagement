using GymManagement.Application.Interfaces.Controllers.DTOs;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GymManagement.Presentation.Api.Controllers.Admin.Users
{
    /// <summary>
    /// Controller for managing user subscriptions and payments (Admin access required)
    /// </summary>
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Roles = RoleConstants.Staff + ", " + RoleConstants.Manager)]
    [SwaggerTag("User Subscriptions - Admin")]
    public class UserSubscribtionController(IUserSubscriptionService _userSubscriptionService) : GymBaseController
    {
        /// <summary>
        /// Subscribe a user to a membership plan
        /// </summary>
        /// <param name="userId">The ID of the user to subscribe</param>
        /// <param name="subscribeUserDto">Subscription details including plan and club</param>
        /// <returns>Created subscription information</returns>
        /// <response code="200">User subscribed successfully</response>
        /// <response code="400">Invalid subscription data</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="404">User or membership plan not found</response>
        /// <response code="409">User already has an active subscription</response>
        [HttpPost("{userId}/subscribe")]
        [SwaggerOperation(Summary = "Subscribe user to membership plan", Description = "Creates a new subscription for a user to a specific membership plan")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
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

        /// <summary>
        /// Process payment for a user's membership subscription
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <param name="membershipId">The ID of the membership to pay for</param>
        /// <param name="paymentDto">Payment details including amount and method</param>
        /// <returns>Updated membership details with payment status</returns>
        /// <response code="200">Payment processed successfully</response>
        /// <response code="400">Invalid payment data</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="404">User or membership not found</response>
        /// <response code="409">Payment already processed</response>
        [HttpPost("{userId}/memberships/{membershipId}")]
        [SwaggerOperation(Summary = "Process membership payment", Description = "Processes payment for a user's membership subscription")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> PaySubscription(int userId, int membershipId, [FromBody] PaymentDto paymentDto)
        {
            var paymentResult = await _userSubscriptionService.PayUserSubscriptionAsync(new UserPaymentDto
            {
                UserId = userId,
                MembershipId = membershipId,
                PaymentMethod = paymentDto.PaymentMethod
            });
            if (!paymentResult.Success)
                return ConvertActionResult(paymentResult);

            var membershipDetails = paymentResult.Results;

            return Ok(membershipDetails);
        }
    }
}

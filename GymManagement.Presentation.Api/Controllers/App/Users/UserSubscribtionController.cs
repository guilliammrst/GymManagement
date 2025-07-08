using GymManagement.Application.Interfaces.Controllers.DTOs;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GymManagement.Presentation.Api.Controllers.App.Users
{
    /// <summary>
    /// Controller for user self-subscription operations (App access)
    /// </summary>
    [ApiController]
    [Route("api/app/users")]
    [Authorize(Roles = RoleConstants.None + ", " + RoleConstants.Member + ", " + RoleConstants.Coach + ", " + RoleConstants.Staff + ", " + RoleConstants.Manager)]
    [SwaggerTag("User Self-Subscriptions - App")]
    public class UserSubscribtionController(IUserVerificationService _userVerificationService, 
        IUserSubscriptionService _userSubscriptionService) : GymBaseController
    {
        /// <summary>
        /// Subscribe the authenticated user to a membership plan
        /// </summary>
        /// <param name="userId">The ID of the user (must match authenticated user)</param>
        /// <param name="subscribeUserDto">Subscription details</param>
        /// <returns>Created subscription information</returns>
        /// <response code="200">User subscribed successfully</response>
        /// <response code="400">Invalid subscription data</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="403">Cannot subscribe another user</response>
        /// <response code="404">User or membership plan not found</response>
        /// <response code="409">User already has an active subscription</response>
        [HttpPost("{userId}/subscribe")]
        [SwaggerOperation(Summary = "Subscribe to membership plan", Description = "Allows a user to subscribe themselves to a membership plan")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> SubscribeUser(int userId, [FromBody] SubscribeUserDto subscribeUserDto)
        {
            var verificationResult = await _userVerificationService.VerifyAuthenticatedUser(userId, User);
            if (!verificationResult.Success)
                return ConvertActionResult(verificationResult);

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
        /// Pay for the authenticated user's own membership subscription
        /// </summary>
        /// <param name="userId">The ID of the user (must match authenticated user)</param>
        /// <param name="membershipId">The ID of the membership to pay for</param>
        /// <param name="paymentDto">Payment details</param>
        /// <returns>Updated membership details with payment status</returns>
        /// <response code="200">Payment processed successfully</response>
        /// <response code="400">Invalid payment data</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="403">Cannot pay for another user's membership</response>
        /// <response code="404">User or membership not found</response>
        /// <response code="409">Payment already processed</response>
        [HttpPost("{userId}/memberships/{membershipId}")]
        [SwaggerOperation(Summary = "Pay for own membership", Description = "Allows a user to pay for their own membership subscription")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> PaySubscription(int userId, int membershipId, [FromBody] PaymentDto paymentDto)
        {
            var verificationResult = await _userVerificationService.VerifyAuthenticatedUser(userId, User);
            if (!verificationResult.Success)
                return ConvertActionResult(verificationResult);

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

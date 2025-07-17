using GymManagement.Application.Interfaces.Controllers.DTOs;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Presentation.Api.Controllers.App.Users
{
    [ApiController]
    [Route("api/app/users")]
    [Authorize(Roles = RoleConstants.None + ", " + RoleConstants.Member + ", " + RoleConstants.Coach + ", " + RoleConstants.Staff + ", " + RoleConstants.Manager)]
    public class UserCoachingController(IUserVerificationService _userVerificationService,
        IUserCoachingService _userCoachingService) : GymBaseController
    {
        [HttpPost("{userId}/coachings")]
        public async Task<ActionResult> SubscribeToCoachingUser(int userId, [FromBody] CoachingUserDto coachingUserDto)
        {
            var verificationResult = await _userVerificationService.VerifyAuthenticatedUser(userId, User);
            if (!verificationResult.Success)
                return ConvertActionResult(verificationResult);

            var addCoachingResult = await _userCoachingService.AddUserCoachingAsync(new UserCoachingDto
            {
                MemberId = userId,
                CoachingPlanId = coachingUserDto.CoachingPlanId,
                StartDate = coachingUserDto.StartDate,
                RenewWhenExpiry = coachingUserDto.RenewWhenExpiry,
                WeekDay = coachingUserDto.WeekDay,
                Hour = coachingUserDto.Hour
            });
            if (!addCoachingResult.Success)
                return ConvertActionResult(addCoachingResult);

            var coachingDetails = addCoachingResult.Results;

            return Ok(coachingDetails);
        }


        [HttpPost("{userId}/coachings/{coachingId}")]
        public async Task<ActionResult> PayCoaching(int userId, int coachingId, [FromBody] PaymentDto paymentDto)
        {
            var verificationResult = await _userVerificationService.VerifyAuthenticatedUser(userId, User);
            if (!verificationResult.Success)
                return ConvertActionResult(verificationResult);

            var paymentResult = await _userCoachingService.PayUserCoachingAsync(new UserPaymentDto
            {
                UserId = userId,
                EntityId = coachingId,
                PaymentMethod = paymentDto.PaymentMethod
            });
            if (!paymentResult.Success)
                return ConvertActionResult(paymentResult);

            var membershipDetails = paymentResult.Results;

            return Ok(membershipDetails);
        }
    }
}

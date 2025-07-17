using GymManagement.Application.Interfaces.Services.Coachings;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Presentation.Api.Controllers.App.Coachings
{
    [ApiController]
    [Route("api/app")]
    [Authorize(Roles = RoleConstants.None + ", " + RoleConstants.Member + ", " + RoleConstants.Coach + ", " + RoleConstants.Staff + ", " + RoleConstants.Manager)]
    public class CoachingQueryController(IUserVerificationService _userVerificationService, 
        ICoachingQueryService _coachingQueryService) : GymBaseController
    {
        [HttpGet("users/{userId}/coachings/{coachingId}")]
        public async Task<ActionResult> GetCoachingPlanById([FromRoute] int userId, [FromRoute] int coachingId)
        {
            var verificationResult = await _userVerificationService.VerifyAuthenticatedUser(userId, User);
            if (!verificationResult.Success)
                return ConvertActionResult(verificationResult);

            var coachingResult = await _coachingQueryService.GetCoachingByIdAsync(coachingId);
            if (!coachingResult.Success)
                return ConvertActionResult(coachingResult);

            var coaching = coachingResult.Results;

            return Ok(coaching);
        }

        [HttpGet("users/{userId}/coachings")]
        public async Task<ActionResult> GetCoachingPlans([FromRoute] int userId)
        {
            var verificationResult = await _userVerificationService.VerifyAuthenticatedUser(userId, User);
            if (!verificationResult.Success)
                return ConvertActionResult(verificationResult);

            var coachingsResult = await _coachingQueryService.GetCoachingsAsync(new CoachingServiceFilter
            {
                MemberId = userId
            });
            if (!coachingsResult.Success)
                return ConvertActionResult(coachingsResult);

            var coachings = coachingsResult.Results;

            return Ok(coachings);
        }
    }
}

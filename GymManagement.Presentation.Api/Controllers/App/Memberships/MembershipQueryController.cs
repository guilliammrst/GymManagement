using GymManagement.Application.Interfaces.Services.Memberships;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Presentation.Api.Controllers.App.Memberships
{
    [ApiController]
    [Route("api/app")]
    [Authorize(Roles = RoleConstants.None + ", " + RoleConstants.Member + ", " + RoleConstants.Coach + ", " + RoleConstants.Staff + ", " + RoleConstants.Manager)]
    public class MembershipQueryController(IUserVerificationService _userVerificationService,
        IMembershipQueryService _membershipQueryService) : GymBaseController
    { 
        [HttpGet("users/{userId}/memberships/{membershipId}")]
        public async Task<ActionResult> GetMembershipById(int userId, int membershipId)
        {
            var verificationResult = await _userVerificationService.VerifyAuthenticatedUser(userId, User);
            if (!verificationResult.Success)
                return ConvertActionResult(verificationResult);

            var membershipResult = await _membershipQueryService.GetMembershipByIdAsync(userId, membershipId);
            if (!membershipResult.Success)
                return ConvertActionResult(membershipResult);

            var membershipDetails = membershipResult.Results;

            return Ok(membershipDetails);
        }
    }
}

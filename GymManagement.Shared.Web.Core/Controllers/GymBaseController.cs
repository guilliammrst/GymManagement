using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Shared.Web.Core.Controllers
{
    public abstract class GymBaseController : ControllerBase
    {
        protected ActionResult ConvertActionResult(IActionResult<GymFaultType> actionResult)
        {
            return ConvertActionResult(actionResult.Fault, actionResult.ErrorMessage);
        }

        protected ActionResult ConvertActionResult(GymFaultType faultType)
        {
            return ConvertActionResult(faultType, string.Empty);
        }

        protected ActionResult ConvertActionResult(GymFaultType faultType, string errorMessage)
        {
            var error = new ErrorDto(faultType, errorMessage);
            switch (faultType)
            {
                case GymFaultType.BadParameter:
                case GymFaultType.InvalidEmailOrPassword:
                case GymFaultType.UserCreationFailed:
                case GymFaultType.ClubCreationFailed:
                case GymFaultType.AddressCreationFailed:
                case GymFaultType.MembershipPlanCreationFailed:
                case GymFaultType.UserEmailAlreadyUsed:
                case GymFaultType.UserPhoneNumberAlreadyUsed:
                case GymFaultType.GetEmailFromTokenFailed:
                case GymFaultType.UserUpdateFailed:
                case GymFaultType.UserAlreadyHaveAnActiveMembership:
                case GymFaultType.UserSubscriptionFailed:
                case GymFaultType.MembershipAlreadyPaid:
                case GymFaultType.MembershipExpired:
                case GymFaultType.MembershipPaymentFailed:
                case GymFaultType.UserDeletionFailed:
                case GymFaultType.ClubUpdateFailed:
                case GymFaultType.MembershipPlanUpdateFailed:
                case GymFaultType.UserHasNoActiveMembership:
                case GymFaultType.UserHasNoMembership:
                case GymFaultType.QrCodeExpired:
                case GymFaultType.UserAddCoachingFailed:
                case GymFaultType.CoachingPlanCreationFailed:
                case GymFaultType.CoachingCreationFailed:
                    return BadRequest(error);

                case GymFaultType.CoachingPaymentDetailNotFound:
                case GymFaultType.CoachingNotFound:
                case GymFaultType.UserNotFound:
                case GymFaultType.ClubNotFound:
                case GymFaultType.ClubManagerNotFound:
                case GymFaultType.AddressNotFound:
                case GymFaultType.MembershipPlanNotFound:
                case GymFaultType.MembershipNotFound:
                case GymFaultType.MembershipPaymentDetailNotFound:
                case GymFaultType.GetTokenFailed:
                case GymFaultType.CoachingPlanNotFound:
                    return NotFound(error);

                case GymFaultType.DatabaseUnavailable:
                    return StatusCode(503, error);

                case GymFaultType.UserNotAuthorized:
                    return StatusCode(403, error);

                case GymFaultType.TokenValidationFailed:
                    return StatusCode(401, error);

                default:
                    return StatusCode(500, error);
            }
        }
    }
}

﻿using GymManagement.Shared.Core.Enums;
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
                case GymFaultType.UserEmailAlreadyUsed:
                case GymFaultType.UserPhoneNumberAlreadyUsed:
                case GymFaultType.GetEmailFromTokenFailed:
                    return BadRequest(error);
                case GymFaultType.UserNotFound:
                    return NotFound(error);
                case GymFaultType.DatabaseUnavailable:
                    return StatusCode(503, error);
                default:
                    return StatusCode(500, error);
            }
        }
    }
}

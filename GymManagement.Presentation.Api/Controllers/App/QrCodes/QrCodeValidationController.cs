using GymManagement.Application.Interfaces.Controllers.DTOs;
using GymManagement.Application.Interfaces.Services.QrCodes;
using GymManagement.Shared.Core.JwtValidation;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Presentation.Api.Controllers.App.QrCodes
{
    [ApiController]
    [Route("api/validate-qr-code")]
    public class QrCodeValidationController(IJwtValidationService _jwtValidationService,
        IQrCodeValidationService _qrCodeValidationService) : GymBaseController
    {
        [HttpPost]
        public ActionResult ValidateQrCode([FromBody] QrCodePayload payload)
        {
            var jwtValidationResult = _jwtValidationService.ValidateToken(payload.Token);
            if (!jwtValidationResult.Success)
                return ConvertActionResult(jwtValidationResult);

            var qrCodeValidationResult = _qrCodeValidationService.Validate(new QrCodeDto
            {
                UserId = payload.UserId,
                Token = payload.Token,
                ExpiresAt = payload.ExpiresAt
            });
            if (!qrCodeValidationResult.Success)
                return ConvertActionResult(qrCodeValidationResult);

            return NoContent();
        }
    }
}

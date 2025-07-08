using GymManagement.Application.Interfaces.Controllers.DTOs;
using GymManagement.Application.Interfaces.Services.QrCodes;
using GymManagement.Shared.Core.JwtValidation;
using GymManagement.Shared.Web.Core.Controllers;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GymManagement.Presentation.Api.Controllers.App.QrCodes
{
    /// <summary>
    /// Controller for QR code validation (App access)
    /// </summary>
    [ApiController]
    [Route("api/app/validate-qr-code")]
    [SwaggerTag("QR Code Validation - App")]
    public class QrCodeValidationController(IJwtValidationService _jwtValidationService,
        IQrCodeValidationService _qrCodeValidationService) : GymBaseController
    {
        /// <summary>
        /// Validates a QR code for gym access
        /// </summary>
        /// <param name="payload">QR code payload containing user ID, token, and expiration</param>
        /// <returns>No content if validation successful</returns>
        /// <response code="204">QR code is valid and access granted</response>
        /// <response code="400">Invalid QR code payload or format</response>
        /// <response code="401">Invalid or expired token</response>
        /// <response code="403">QR code validation failed</response>
        /// <response code="404">User not found</response>
        [HttpPost]
        [SwaggerOperation(Summary = "Validate QR code for gym access", Description = "Validates a QR code and grants or denies gym access")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

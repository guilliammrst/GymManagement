using GymManagement.Application.Interfaces.Services.QrCodes;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Web.Core.Controllers;
using GymManagement.Shared.Web.Core.RequestExt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GymManagement.Presentation.Api.Controllers.App.QrCodes
{
    /// <summary>
    /// Controller for QR code generation (App access)
    /// </summary>
    [ApiController]
    [Route("api/app/users")]
    [Authorize(Roles = RoleConstants.None + ", " + RoleConstants.Member + ", " + RoleConstants.Coach + ", " + RoleConstants.Staff + ", " + RoleConstants.Manager)]
    [SwaggerTag("QR Code Generation - App")]
    public class QrCodeGenerationController(IUserVerificationService _userVerificationService,
        IQrCodeGenerationService _qrCodeGenerationService) : GymBaseController
    {
        /// <summary>
        /// DEBUG: Check token claims and user information
        /// </summary>
        /// <returns>Token claims information</returns>
        [HttpGet("debug-token")]
        [SwaggerOperation(Summary = "DEBUG: Check token claims", Description = "Debug endpoint to check token claims and user information")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult DebugToken()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            var roles = User.Claims.Where(c => c.Type == ClaimsTypes.Role).Select(c => c.Value).ToList();
            var isInNoneRole = User.IsInRole(RoleConstants.None);
            
            return Ok(new
            {
                AllClaims = claims,
                Roles = roles,
                IsInNoneRole = isInNoneRole,
                RoleConstantNone = RoleConstants.None,
                ClaimTypesRole = ClaimsTypes.Role,
                IsAuthenticated = User.Identity.IsAuthenticated,
                AuthenticationType = User.Identity.AuthenticationType
            });
        }

        /// <summary>
        /// Generates a QR code for the authenticated user's gym access
        /// </summary>
        /// <param name="userId">The ID of the user (must match authenticated user)</param>
        /// <returns>QR code as SVG image</returns>
        /// <response code="200">QR code generated successfully</response>
        /// <response code="401">Unauthorized access or invalid token</response>
        /// <response code="403">Cannot generate QR code for another user</response>
        /// <response code="404">User not found</response>
        /// <response code="400">Invalid token in request</response>
        [HttpGet("{userId}/qr-code")]
        [SwaggerOperation(Summary = "Generate QR code for gym access", Description = "Generates a QR code that can be used for gym access validation")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK, "image/svg+xml")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GenerateQrCode(int userId)
        {
            var tokenResult = Request.GetToken();
            if (!tokenResult.Success)
                return ConvertActionResult(tokenResult);

            var token = tokenResult.Results;

            var verificationResult = await _userVerificationService.VerifyAuthenticatedUser(userId, User);
            if (!verificationResult.Success)
                return ConvertActionResult(verificationResult);

            var qrCodeResult = await _qrCodeGenerationService.GenerateQrCodeAsync(userId, token);
            if (!qrCodeResult.Success)
                return ConvertActionResult(qrCodeResult);

            var qrCode = qrCodeResult.Results;

            return Content(qrCode, "image/svg+xml");
        }
    }
}

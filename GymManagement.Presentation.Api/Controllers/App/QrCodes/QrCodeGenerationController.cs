using GymManagement.Application.Interfaces.Services.QrCodes;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Web.Core.Controllers;
using GymManagement.Shared.Web.Core.RequestExt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Presentation.Api.Controllers.App.QrCodes
{
    [ApiController]
    [Route("api/app/users")]
    [Authorize(Roles = RoleConstants.None + ", " + RoleConstants.Member + ", " + RoleConstants.Coach + ", " + RoleConstants.Staff + ", " + RoleConstants.Manager)]
    public class QrCodeGenerationController(IUserVerificationService _userVerificationService,
        IQrCodeGenerationService _qrCodeGenerationService) : GymBaseController
    {
        [HttpGet("{userId}/qr-code")]
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

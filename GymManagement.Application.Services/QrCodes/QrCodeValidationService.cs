using GymManagement.Application.Interfaces.Services.QrCodes;
using GymManagement.Domain;
using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Services.QrCodes
{
    public class QrCodeValidationService : IQrCodeValidationService
    {
        public ModelActionResult Validate(QrCodeDto qrCodeDto)
        {
            var qrCodeResult = QrCode.Build(qrCodeDto.UserId, qrCodeDto.Token, qrCodeDto.ExpiresAt);
            if (!qrCodeResult.Success)
                return ModelActionResult.Fail(qrCodeResult);

            var qrCode = qrCodeResult.Results;

            return qrCode.Validate();
        }
    }
}

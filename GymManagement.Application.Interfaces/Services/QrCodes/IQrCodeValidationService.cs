using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Services.QrCodes
{
    public interface IQrCodeValidationService
    {
        ModelActionResult Validate(QrCodeDto qrCodeDto);
    }
}

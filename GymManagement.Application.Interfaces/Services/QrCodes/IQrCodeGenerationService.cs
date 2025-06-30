using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Services.QrCodes
{
    public interface IQrCodeGenerationService
    {
        Task<ModelActionResult<string>> GenerateQrCodeAsync(int userId, string token);
    }
}

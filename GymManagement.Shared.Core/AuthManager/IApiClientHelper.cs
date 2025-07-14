using GymManagement.Shared.Core.Results;

namespace GymManagement.Shared.Core.AuthManager
{
    public interface IApiClientHelper
    {
        Task<ModelActionResult> GenerateActionResult(HttpResponseMessage response);
        Task<ModelActionResult> CheckAuthenticatedUser();
    }
}
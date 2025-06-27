using GymManagement.Shared.Core.Results;

namespace GymManagement.Presentation.WebApp.ApiClients
{
    public interface IApiClientHelper
    {
        Task<ModelActionResult> GenerateActionResult(HttpResponseMessage response);
        Task<ModelActionResult> CheckAuthenticatedUser();
    }
}
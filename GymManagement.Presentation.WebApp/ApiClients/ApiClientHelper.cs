using Azure;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;
using GymManagement.Shared.Web.Core.Controllers;

namespace GymManagement.Presentation.WebApp.ApiClients
{
    public static class ApiClientHelper
    {
        public static async Task<ModelActionResult> GenerateActionResult(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorDto = await response.Content.ReadFromJsonAsync<ErrorDto>();
                return ModelActionResult.Fail((GymFaultType)errorDto?.FaultCode!, errorDto?.Message!);
            }
            return ModelActionResult.Ok;
        }
    }
}

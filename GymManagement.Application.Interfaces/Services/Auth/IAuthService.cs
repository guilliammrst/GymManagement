using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Services.Auth
{
    public interface IAuthService
    {
        Task<ModelActionResult<string>> Login(LoginDto loginDto);
    }
}
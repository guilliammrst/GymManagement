using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Services.Auth
{
    public interface IAuthService
    {
        Task<ModelActionResult<string>> LoginAsync(LoginDto loginDto);
        Task<ModelActionResult<string>> RegisterAsync(RegisterDto registerDto);
    }
}
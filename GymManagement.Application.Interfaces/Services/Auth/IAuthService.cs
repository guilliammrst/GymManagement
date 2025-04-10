using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Services.Auth
{
    public interface IAuthService
    {
        Task<ModelActionResult> Login(LoginDto loginDto);
        Task<ModelActionResult<string>> GenerateToken(string? email);
    }
}
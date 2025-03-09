using GymManagement.Web.Core.Results;

namespace GymManagement.Services.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<ModelActionResult> Login(LoginDto loginDto);
        Task<ModelActionResult<string>> GenerateToken(string? email);
    }
}
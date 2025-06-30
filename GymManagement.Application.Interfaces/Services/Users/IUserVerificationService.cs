using GymManagement.Shared.Core.Results;
using System.Security.Claims;

namespace GymManagement.Application.Interfaces.Services.Users
{
    public interface IUserVerificationService
    {
        Task<ModelActionResult> VerifyAuthenticatedUser(int userId, ClaimsPrincipal claimsPrincipal);
    }
}

using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Services.Users
{
    public interface IUserQueryService
    {
        Task<ModelActionResult<UserDetailsDto>> GetUserByIdAsync(int id);
        Task<ModelActionResult<List<UserDto>>> GetUsersAsync(UserServiceFilter filter);
        Task<ModelActionResult<UserDetailsDto>> MeAsync(string email);
    }
}

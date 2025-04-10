using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Repositories.Users
{
    public interface IUserRepository
    {
        Task<ModelActionResult<UserDetailsDao>> GetUserByIdAsync(int id);
        Task<ModelActionResult<List<UserDao>>> GetUsersAsync();
        Task<ModelActionResult<UserDao>> CreateUserAsync(UserCreateDao userCreate);
    }
}
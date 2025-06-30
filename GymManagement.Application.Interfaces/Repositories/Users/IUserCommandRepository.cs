using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Repositories.Users
{
    public interface IUserCommandRepository
    {
        Task<ModelActionResult<UserDao>> CreateUserAsync(UserCreateDao userCreateDto);
        Task<ModelActionResult<UserDao>> UpdateUserAsync(UserUpdateDao userUpdateDto);
        Task<ModelActionResult> DeleteUserAsync(int userId);
    }
}

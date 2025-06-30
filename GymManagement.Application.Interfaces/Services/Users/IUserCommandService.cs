using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Services.Users
{
    public interface IUserCommandService
    {
        Task<ModelActionResult<UserDto>> CreateUserAsync(UserCreateDto userCreateDto);
        Task<ModelActionResult<UserDto>> UpdateUserAsync(UserUpdateDto userUpdateDto);
        Task<ModelActionResult> DeleteUserAsync(int userId);
    }
}

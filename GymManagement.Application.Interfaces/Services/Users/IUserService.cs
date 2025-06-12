using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Services.Users
{
    public interface IUserService
    {
        Task<ModelActionResult<UserDetailsDto>> GetUserByIdAsync(int id);
        Task<ModelActionResult<List<UserDto>>> GetUsersAsync();
        Task<ModelActionResult<UserDto>> CreateUserAsync(UserCreateDto userCreateDto);
        Task<ModelActionResult<UserDetailsDto>> UpdateUserAsync(UserUpdateDto userUpdateDto);
    }
}

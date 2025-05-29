using GymManagement.Domain;
using GymManagement.Application.Services.Converters;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Shared.Core.Results;
using GymManagement.Application.Interfaces.Repositories.Users;

namespace GymManagement.Application.Services.Users
{
    public class UserService(IUserRepository _userRepository) : IUserService
    {
        public async Task<ModelActionResult<UserDetailsDto>> GetUserByIdAsync(int id)
        {
            var userResult = await _userRepository.GetUserByIdAsync(id);
            if (!userResult.Success)
                return ModelActionResult<UserDetailsDto>.Fail(userResult);

            var user = userResult.Results;

            return ModelActionResult<UserDetailsDto>.Ok(user.ToDetailsDto());
        }

        public async Task<ModelActionResult<List<UserDto>>> GetUsersAsync()
        {
            var usersResult = await _userRepository.GetUsersAsync();
            if (!usersResult.Success)
                return ModelActionResult<List<UserDto>>.Fail(usersResult);

            var users = usersResult.Results;

            return ModelActionResult<List<UserDto>>.Ok(users.ToDto());
        }

        public async Task<ModelActionResult<UserDto>> CreateUserAsync (UserCreateDto userCreateDto)
        {
            var userCreateResult = User.Create(userCreateDto.Name, userCreateDto.Surname, userCreateDto.Birthdate, userCreateDto.Password,
                userCreateDto.Role, userCreateDto.Email, userCreateDto.PhoneNumber, userCreateDto.Gender);
            if (!userCreateResult.Success)
                return ModelActionResult<UserDto>.Fail(userCreateResult);

            var user = userCreateResult.Results;

            var newUserResult = await _userRepository.CreateUserAsync(user.ToCreateDao());
            if (!newUserResult.Success)
                return ModelActionResult<UserDto>.Fail(newUserResult);

            var newUser = newUserResult.Results;

            return ModelActionResult<UserDto>.Ok(newUser.ToDto());
        }
    }
}

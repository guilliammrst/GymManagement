using GymManagement.Application.Interfaces.Repositories.Users;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Application.Services.Converters;
using GymManagement.Domain;
using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Services.Users
{
    public class UserCommandService(IUserCommandRepository _userCommandRepository, IUserQueryRepository _userQueryRepository) : IUserCommandService
    {
        public async Task<ModelActionResult<UserDto>> CreateUserAsync(UserCreateDto userCreateDto)
        {
            var userCreateResult = User.Create(userCreateDto.Name, userCreateDto.Surname, userCreateDto.Birthdate, userCreateDto.Password,
                userCreateDto.Role, userCreateDto.Email, userCreateDto.PhoneNumber, userCreateDto.Gender, userCreateDto.Country,
                userCreateDto.City, userCreateDto.Street, userCreateDto.PostalCode, userCreateDto.Number);
            if (!userCreateResult.Success)
                return ModelActionResult<UserDto>.Fail(userCreateResult);

            var user = userCreateResult.Results;

            var newUserResult = await _userCommandRepository.CreateUserAsync(user.ToCreateDao());
            if (!newUserResult.Success)
                return ModelActionResult<UserDto>.Fail(newUserResult);

            var newUser = newUserResult.Results;

            return ModelActionResult<UserDto>.Ok(newUser.ToDto());
        }

        public async Task<ModelActionResult<UserDto>> UpdateUserAsync(UserUpdateDto userUpdateDto)
        {
            var userResult = await _userQueryRepository.GetUserByIdAsync(userUpdateDto.Id);

            if (!userResult.Success)
                return ModelActionResult<UserDto>.Fail(userResult);

            var userDao = userResult.Results;

            var userBuildResult = User.Build(userDao.Id, userDao.CreatedAt, userDao.UpdatedAt, userDao.Name, userDao.Surname, userDao.Birthdate, userDao.Password,
                userDao.Role, userDao.Email, userDao.PhoneNumber, userDao.Gender, userDao.Address.Country,
                userDao.Address.City, userDao.Address.Street, userDao.Address.PostalCode, userDao.Address.Number);
            if (!userBuildResult.Success)
                return ModelActionResult<UserDto>.Fail(userBuildResult);

            var user = userBuildResult.Results;

            var updateUserResult = user.Update(userUpdateDto.Name, userUpdateDto.Surname, userUpdateDto.Birthdate, userUpdateDto.Password,
                userUpdateDto.Role, userUpdateDto.Email, userUpdateDto.PhoneNumber, userUpdateDto.Gender, userUpdateDto.Country,
                userUpdateDto.City, userUpdateDto.Street, userUpdateDto.PostalCode, userUpdateDto.Number);
            if (!updateUserResult.Success)
                return ModelActionResult<UserDto>.Fail(updateUserResult);

            var userUpdatedResult = await _userCommandRepository.UpdateUserAsync(user.ToUpdateDao());
            if (!userUpdatedResult.Success)
                return ModelActionResult<UserDto>.Fail(userUpdatedResult);

            var updatedUser = userUpdatedResult.Results;

            return ModelActionResult<UserDto>.Ok(updatedUser.ToDto());
        }

        public async Task<ModelActionResult> DeleteUserAsync(int userId)
        {
            var deleteUserResult = await _userCommandRepository.DeleteUserAsync(userId);
            if (!deleteUserResult.Success)
                return ModelActionResult.Fail(deleteUserResult);

            return ModelActionResult.Ok;
        }
    }
}

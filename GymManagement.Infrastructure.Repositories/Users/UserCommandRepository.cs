using GymManagement.Application.Interfaces.Repositories.Users;
using GymManagement.Infrastructure.Repositories.Converters;
using GymManagement.Infrastructure.Repositories.DbContexts;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repositories.Users
{
    public class UserCommandRepository(GymDbContext _context) : IUserCommandRepository
    {
        public async Task<ModelActionResult<UserDao>> CreateUserAsync(UserCreateDao userCreate)
        {
            try
            {
                if (_context.Users.Any(u => u.Email == userCreate.Email))
                    return ModelActionResult<UserDao>.Fail(GymFaultType.UserEmailAlreadyUsed, "User creation failed: user email already used.");

                if (_context.Users.Any(u => u.PhoneNumber == userCreate.PhoneNumber))
                    return ModelActionResult<UserDao>.Fail(GymFaultType.UserPhoneNumberAlreadyUsed, "User creation failed: user phone number already used.");

                var userModel = userCreate.ToModel();

                await _context.Users.AddAsync(userModel);
                var result = await _context.SaveChangesAsync();
                if (result == 0)
                    return ModelActionResult<UserDao>.Fail(GymFaultType.UserCreationFailed, "User creation failed: no rows affected.");

                return ModelActionResult<UserDao>.Ok(userModel.ToDao());
            }
            catch (DbUpdateException ex)
            {
                return ModelActionResult<UserDao>.Fail(GymFaultType.DatabaseUnavailable, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
            catch (Exception ex)
            {
                return ModelActionResult<UserDao>.Fail(GymFaultType.UserCreationFailed, ex.Message);
            }
        }

        public async Task<ModelActionResult<UserDao>> UpdateUserAsync(UserUpdateDao userUpdateDto)
        {
            try
            {
                var userModel = await _context.Users
                    .Include(u => u.Address)
                    .FirstOrDefaultAsync(u => u.Id == userUpdateDto.Id);

                if (userModel == null)
                    return ModelActionResult<UserDao>.Fail(GymFaultType.UserNotFound, "User update failed: user not found.");

                if (_context.Users.Any(u => u.Email == userUpdateDto.Email && u.Id != userUpdateDto.Id))
                    return ModelActionResult<UserDao>.Fail(GymFaultType.UserEmailAlreadyUsed, "User update failed: user email already used.");

                if (_context.Users.Any(u => u.PhoneNumber == userUpdateDto.PhoneNumber && u.Id != userUpdateDto.Id))
                    return ModelActionResult<UserDao>.Fail(GymFaultType.UserPhoneNumberAlreadyUsed, "User update failed: user phone number already used.");

                if (!string.IsNullOrWhiteSpace(userUpdateDto.Name))
                    userModel.Name = userUpdateDto.Name;

                if (!string.IsNullOrWhiteSpace(userUpdateDto.Surname))
                    userModel.Surname = userUpdateDto.Surname;

                if (userUpdateDto.Birthdate.HasValue)
                    userModel.Birthdate = userUpdateDto.Birthdate.Value;

                if (!string.IsNullOrWhiteSpace(userUpdateDto.Password))
                    userModel.Password = userUpdateDto.Password;

                if (userUpdateDto.Role.HasValue)
                    userModel.Role = userUpdateDto.Role.Value;

                if (!string.IsNullOrWhiteSpace(userUpdateDto.Email))
                    userModel.Email = userUpdateDto.Email;

                if (!string.IsNullOrWhiteSpace(userUpdateDto.PhoneNumber))
                    userModel.PhoneNumber = userUpdateDto.PhoneNumber;

                if (userUpdateDto.Gender.HasValue)
                    userModel.Gender = userUpdateDto.Gender.Value;

                if (userUpdateDto.Country.HasValue && userModel.Address != null)
                    userModel.Address.Country = userUpdateDto.Country.Value;

                if (!string.IsNullOrWhiteSpace(userUpdateDto.City) && userModel.Address != null)
                    userModel.Address.City = userUpdateDto.City;

                if (!string.IsNullOrWhiteSpace(userUpdateDto.Street) && userModel.Address != null)
                    userModel.Address.Street = userUpdateDto.Street;

                if (!string.IsNullOrWhiteSpace(userUpdateDto.PostalCode) && userModel.Address != null)
                    userModel.Address.PostalCode = userUpdateDto.PostalCode;

                if (!string.IsNullOrWhiteSpace(userUpdateDto.Number) && userModel.Address != null)
                    userModel.Address.Number = userUpdateDto.Number;

                _context.Users.Update(userModel);

                var result = await _context.SaveChangesAsync();
                if (result == 0)
                    return ModelActionResult<UserDao>.Fail(GymFaultType.UserUpdateFailed, "User update failed: no rows affected.");

                return ModelActionResult<UserDao>.Ok(userModel.ToDao());
            }
            catch (DbUpdateException ex)
            {
                return ModelActionResult<UserDao>.Fail(GymFaultType.DatabaseUnavailable, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
            catch (Exception ex)
            {
                return ModelActionResult<UserDao>.Fail(GymFaultType.UserUpdateFailed, ex.Message);
            }
        }
    }
}

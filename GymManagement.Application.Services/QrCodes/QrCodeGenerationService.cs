using GymManagement.Application.Interfaces.Repositories.Users;
using GymManagement.Application.Interfaces.Services.QrCodes;
using GymManagement.Application.Services.Converters;
using GymManagement.Domain;
using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Services.QrCodes
{
    public class QrCodeGenerationService(IUserQueryRepository _userQueryRepository) : IQrCodeGenerationService
    {
        public async Task<ModelActionResult<string>> GenerateQrCodeAsync(int userId, string token)
        {
            var userDaoResult = await _userQueryRepository.GetUserByIdAsync(userId);
            if (!userDaoResult.Success)
                return ModelActionResult<string>.Fail(userDaoResult);

            var userDao = userDaoResult.Results;

            var userResult = User.Build(userDao.Id, userDao.CreatedAt, userDao.UpdatedAt, userDao.Name, userDao.Surname, userDao.Birthdate, userDao.Password,
                userDao.Role, userDao.Email, userDao.PhoneNumber, userDao.Gender, userDao.Address.Country,
                userDao.Address.City, userDao.Address.Street, userDao.Address.PostalCode, userDao.Address.Number, userDao.Memberships.Build());
            if (!userResult.Success)
                return ModelActionResult<string>.Fail(userResult);

            var user = userResult.Results;

            var qrCodeGenerationResult = user.GenerateQrCode(token);
            if (!qrCodeGenerationResult.Success)
                return ModelActionResult<string>.Fail(qrCodeGenerationResult);

            var qrCode = qrCodeGenerationResult.Results;

            return ModelActionResult<string>.Ok(qrCode);
        }

        public async Task<ModelActionResult<string>> GenerateQrCodeAsync(string userEmail, string token)
        {
            var userDaoResult = await _userQueryRepository.GetUserByEmailAsync(userEmail);
            if (!userDaoResult.Success)
                return ModelActionResult<string>.Fail(userDaoResult);

            var userDao = userDaoResult.Results;

            var userResult = User.Build(userDao.Id, userDao.CreatedAt, userDao.UpdatedAt, userDao.Name, userDao.Surname, userDao.Birthdate, userDao.Password,
                userDao.Role, userDao.Email, userDao.PhoneNumber, userDao.Gender, userDao.Address.Country,
                userDao.Address.City, userDao.Address.Street, userDao.Address.PostalCode, userDao.Address.Number, userDao.Memberships.Build());
            if (!userResult.Success)
                return ModelActionResult<string>.Fail(userResult);

            var user = userResult.Results;

            var qrCodeGenerationResult = user.GenerateQrCode(token);
            if (!qrCodeGenerationResult.Success)
                return ModelActionResult<string>.Fail(qrCodeGenerationResult);

            var qrCode = qrCodeGenerationResult.Results;

            return ModelActionResult<string>.Ok(qrCode);
        }
    }
}

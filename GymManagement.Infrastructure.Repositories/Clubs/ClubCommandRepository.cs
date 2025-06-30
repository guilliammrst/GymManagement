using GymManagement.Application.Interfaces.Repositories.Addresses;
using GymManagement.Application.Interfaces.Repositories.Clubs;
using GymManagement.Infrastructure.Repositories.Converters;
using GymManagement.Infrastructure.Repositories.DbContexts;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repositories.Clubs
{
    public class ClubCommandRepository(GymDbContext _context, IAddressCommandRepository _addressCommandRepository) : IClubCommandRepository
    {
        public async Task<ModelActionResult<ClubDetailsDao>> CreateClubAsync(ClubCreateDao clubCreateDao)
        {
            try
            {
                var addressResult = await _addressCommandRepository.CreateAddressAsync(clubCreateDao.ToAddressCreate());
                if (!addressResult.Success)
                    return ModelActionResult<ClubDetailsDao>.Fail(addressResult);

                var address = await _context.Addresses.FindAsync(addressResult.Results.Id);
                if (address == null)
                    return ModelActionResult<ClubDetailsDao>.Fail(GymFaultType.AddressNotFound, "Club creation failed: Address not found.");

                var clubModel = clubCreateDao.ToModel();
                clubModel.Address = address;

                if (clubCreateDao.ManagerId.HasValue && clubCreateDao.ManagerId >= 0)
                {
                    var clubManager = await _context.Users.FindAsync(clubCreateDao.ManagerId.Value);
                    if (clubManager == null || clubManager.Role != Role.Manager)
                        return ModelActionResult<ClubDetailsDao>.Fail(GymFaultType.ClubManagerNotFound, "Club creation failed: Manager not found.");

                    clubModel.Manager = clubManager;
                }

                await _context.Clubs.AddAsync(clubModel);
                var result = await _context.SaveChangesAsync();

                if (result == 0)
                    return ModelActionResult<ClubDetailsDao>.Fail(GymFaultType.ClubCreationFailed, "Club creation failed: no rows affected.");

                return ModelActionResult<ClubDetailsDao>.Ok(clubModel.ToDetailsDao());
            }
            catch (DbUpdateException ex)
            {
                return ModelActionResult<ClubDetailsDao>.Fail(GymFaultType.DatabaseUnavailable, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
            catch (Exception ex)
            {
                return ModelActionResult<ClubDetailsDao>.Fail(GymFaultType.ClubCreationFailed, ex.Message);
            }
        }

        public async Task<ModelActionResult> UpdateClubAsync(ClubUpdateDao clubUpdateDao)
        {
            try
            {
                var clubModel = await _context.Clubs
                    .Include(c => c.Address)
                    .FirstOrDefaultAsync(c => c.Id == clubUpdateDao.Id);

                if (clubModel == null)
                    return ModelActionResult.Fail(GymFaultType.ClubNotFound, "Club update failed: club not found.");

                if (!string.IsNullOrWhiteSpace(clubUpdateDao.Name))
                    clubModel.Name = clubUpdateDao.Name;

                if (clubUpdateDao.Country.HasValue && clubModel.Address != null)
                    clubModel.Address.Country = clubUpdateDao.Country.Value;

                if (!string.IsNullOrWhiteSpace(clubUpdateDao.City) && clubModel.Address != null)
                    clubModel.Address.City = clubUpdateDao.City;

                if (!string.IsNullOrWhiteSpace(clubUpdateDao.Street) && clubModel.Address != null)
                    clubModel.Address.Street = clubUpdateDao.Street;

                if (!string.IsNullOrWhiteSpace(clubUpdateDao.PostalCode) && clubModel.Address != null)
                    clubModel.Address.PostalCode = clubUpdateDao.PostalCode;

                if (!string.IsNullOrWhiteSpace(clubUpdateDao.Number) && clubModel.Address != null)
                    clubModel.Address.Number = clubUpdateDao.Number;

                if (clubUpdateDao.ManagerId.HasValue && clubUpdateDao.ManagerId >= 0)
                {
                    var clubManager = await _context.Users.FindAsync(clubUpdateDao.ManagerId.Value);
                    if (clubManager == null || clubManager.Role != Role.Manager)
                        return ModelActionResult.Fail(GymFaultType.ClubManagerNotFound, "Club update failed: Manager not found.");
                    clubModel.Manager = clubManager;
                }

                _context.Clubs.Update(clubModel);

                var result = await _context.SaveChangesAsync();
                if (result == 0)
                    return ModelActionResult.Fail(GymFaultType.ClubUpdateFailed, "Club update failed: no rows affected.");

                return ModelActionResult.Ok;
            }
            catch (DbUpdateException ex)
            {
                return ModelActionResult.Fail(GymFaultType.DatabaseUnavailable, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
            catch (Exception ex)
            {
                return ModelActionResult.Fail(GymFaultType.ClubUpdateFailed, ex.Message);
            }
        }
    }
}

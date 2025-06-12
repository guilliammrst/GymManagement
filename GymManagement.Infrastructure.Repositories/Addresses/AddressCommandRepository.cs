using GymManagement.Application.Interfaces.Repositories.Addresses;
using GymManagement.Infrastructure.Repositories.Converters;
using GymManagement.Infrastructure.Repositories.DbContexts;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repositories.Addresses
{
    public class AddressCommandRepository(GymDbContext _context) : IAddressCommandRepository
    {
        public async Task<ModelActionResult<AddressDao>> CreateAddressAsync(AddressCreateDao addressCreateDao)
        {
            try
            {
                var addressModel = addressCreateDao.ToModel();

                _context.Addresses.Add(addressModel);

                var result = await _context.SaveChangesAsync();
                if (result == 0)
                    return ModelActionResult<AddressDao>.Fail(GymFaultType.AddressCreationFailed, "Address creation failed: no rows affected.");

                return ModelActionResult<AddressDao>.Ok(addressModel.ToDao());
            }
            catch (DbUpdateException ex)
            {
                return ModelActionResult<AddressDao>.Fail(GymFaultType.DatabaseUnavailable, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
            catch (Exception ex)
            {
                return ModelActionResult<AddressDao>.Fail(GymFaultType.AddressCreationFailed, ex.Message);
            }
        }
    }
}

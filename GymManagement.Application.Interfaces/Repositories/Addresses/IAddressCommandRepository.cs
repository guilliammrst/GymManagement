using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Repositories.Addresses
{
    public interface IAddressCommandRepository
    {
        Task<ModelActionResult<AddressDao>> CreateAddressAsync(AddressCreateDao addressCreateDao);
    }
}

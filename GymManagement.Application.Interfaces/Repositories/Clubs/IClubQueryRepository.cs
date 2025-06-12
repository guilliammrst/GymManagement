using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Repositories.Clubs
{
    public interface IClubQueryRepository
    {
        Task<ModelActionResult<ClubDetailsDao>> GetClubByIdAsync(int clubId);
        Task<ModelActionResult<List<ClubDao>>> GetClubsAsync();
    }
}

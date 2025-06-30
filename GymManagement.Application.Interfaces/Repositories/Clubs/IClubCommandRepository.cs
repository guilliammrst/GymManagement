using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Repositories.Clubs
{
    public interface IClubCommandRepository
    {
        Task<ModelActionResult<ClubDetailsDao>> CreateClubAsync(ClubCreateDao clubCreateDao);
        Task<ModelActionResult> UpdateClubAsync(ClubUpdateDao clubUpdateDao);
    }
}

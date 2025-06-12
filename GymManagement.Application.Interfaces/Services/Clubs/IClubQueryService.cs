using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Services.Clubs
{
    public interface IClubQueryService
    {
        Task<ModelActionResult<ClubDetailsDto>> GetClubByIdAsync(int clubId);
        Task<ModelActionResult<List<ClubDto>>> GetClubsAsync();
    }
}

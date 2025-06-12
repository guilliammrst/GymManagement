using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Services.Clubs
{
    public interface IClubCommandService
    {
        Task<ModelActionResult<ClubCreateResult>> CreateClubAsync(ClubCreateDto clubCreatecDto);
    }
}

using GymManagement.Application.Interfaces.Repositories.Clubs;
using GymManagement.Application.Interfaces.Services.Clubs;
using GymManagement.Application.Services.Converters;
using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Services.Clubs
{
    public class ClubQueryService(IClubQueryRepository _clubQueryRepository) : IClubQueryService
    {
        public async Task<ModelActionResult<ClubDetailsDto>> GetClubByIdAsync(int clubId)
        {
            var clubResult = await _clubQueryRepository.GetClubByIdAsync(clubId);
            if (!clubResult.Success)
                return ModelActionResult<ClubDetailsDto>.Fail(clubResult);

            var club = clubResult.Results;

            return ModelActionResult<ClubDetailsDto>.Ok(club.ToDetailsDto());
        }

        public async Task<ModelActionResult<List<ClubDto>>> GetClubsAsync()
        {
            var clubsResult = await _clubQueryRepository.GetClubsAsync();
            if (!clubsResult.Success)
                return ModelActionResult<List<ClubDto>>.Fail(clubsResult);

            var clubs = clubsResult.Results;

            return ModelActionResult<List<ClubDto>>.Ok(clubs.ToDto());
        }
    }
}

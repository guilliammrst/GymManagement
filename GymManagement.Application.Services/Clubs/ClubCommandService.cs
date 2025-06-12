using GymManagement.Application.Interfaces.Repositories.Clubs;
using GymManagement.Application.Interfaces.Services.Clubs;
using GymManagement.Application.Services.Converters;
using GymManagement.Domain;
using GymManagement.Shared.Core.Results;
namespace GymManagement.Application.Services.Clubs
{
    public class ClubCommandService(IClubCommandRepository _clubCommandRepository) : IClubCommandService
    {
        public async Task<ModelActionResult<ClubCreateResult>> CreateClubAsync(ClubCreateDto clubCreatecDto)
        {
            var createClubResult = Club.Create(clubCreatecDto.Name, clubCreatecDto.Country, clubCreatecDto.City, clubCreatecDto.Street, clubCreatecDto.PostalCode, clubCreatecDto.Number, clubCreatecDto.ManagerId);
            if (!createClubResult.Success)
                return ModelActionResult<ClubCreateResult>.Fail(createClubResult);

            var club = createClubResult.Results;
            
            var newClubResult = await _clubCommandRepository.CreateClubAsync(club.ToCreateDao());
            if (!newClubResult.Success)
                return ModelActionResult<ClubCreateResult>.Fail(newClubResult);

            var newClub = newClubResult.Results;

            return ModelActionResult<ClubCreateResult>.Ok(newClub.ToCreateResult());
        }
    }
}

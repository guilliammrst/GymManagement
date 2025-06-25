using GymManagement.Application.Interfaces.Repositories.Clubs;
using GymManagement.Application.Interfaces.Services.Clubs;
using GymManagement.Application.Services.Converters;
using GymManagement.Domain;
using GymManagement.Shared.Core.Results;
namespace GymManagement.Application.Services.Clubs
{
    public class ClubCommandService(IClubCommandRepository _clubCommandRepository, IClubQueryRepository _clubQueryRepository) : IClubCommandService
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

        public async Task<ModelActionResult> UpdateClubAsync(ClubUpdateDto clubCreatecDto)
        {
            var clubResult = await _clubQueryRepository.GetClubByIdAsync(clubCreatecDto.Id);
            if (!clubResult.Success)
                return ModelActionResult.Fail(clubResult);

            var clubDao = clubResult.Results;

            var clubBuildResult = Club.Build(clubDao.Id, clubDao.CreatedAt, clubDao.UpdatedAt, clubDao.Name, clubDao.Address.Country, clubDao.Address.City, clubDao.Address.Street, clubDao.Address.PostalCode, clubDao.Address.Number, clubDao.Manager.Id);
            if (!clubBuildResult.Success)
                return ModelActionResult.Fail(clubBuildResult);

            var club = clubBuildResult.Results;

            var updateClubResult = club.Update(clubCreatecDto.Name, clubCreatecDto.Country, clubCreatecDto.City, clubCreatecDto.Street, clubCreatecDto.PostalCode, clubCreatecDto.Number, clubCreatecDto.ManagerId);
            if (!updateClubResult.Success)
                return ModelActionResult.Fail(updateClubResult);

            var updatedClubResult = await _clubCommandRepository.UpdateClubAsync(club.ToUpdateDao());
            if (!updatedClubResult.Success)
                return ModelActionResult.Fail(updatedClubResult);

            return ModelActionResult.Ok;
        }
    }
}

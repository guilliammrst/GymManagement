using GymManagement.Application.Interfaces.Repositories.Clubs;
using GymManagement.Infrastructure.Repositories.Converters;
using GymManagement.Infrastructure.Repositories.DbContexts;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repositories.Clubs
{
    public class ClubQueryRepository(GymDbContext _context) : IClubQueryRepository
    {
        public async Task<ModelActionResult<ClubDetailsDao>> GetClubByIdAsync(int clubId)
        {
            var clubModel = await _context.Clubs
                .Include(c => c.Address)
                .Include(c => c.Manager)
                .Include(c => c.Attendances)
                .Include(c => c.Memberships)
                .FirstOrDefaultAsync(c => c.Id == clubId);

            if (clubModel == null)
                return ModelActionResult<ClubDetailsDao>.Fail(GymFaultType.ClubNotFound, "Get club failed: club not found.");

            var club = clubModel.ToDetailsDao();
            return ModelActionResult<ClubDetailsDao>.Ok(club);
        }

        public async Task<ModelActionResult<List<ClubDao>>> GetClubsAsync()
        {
            var clubs = await _context.Clubs
                .Include(c => c.Address)
                .ToListAsync();

            return ModelActionResult<List<ClubDao>>.Ok(clubs.ToDao());
        }
    }
}

using GymManagement.Application.Interfaces.Repositories.CoachingPlans;
using GymManagement.Infrastructure.Repositories.Converters;
using GymManagement.Infrastructure.Repositories.DbContexts;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repositories.CoachingPlans
{
    public class CoachingPlanCommandRepository(GymDbContext _context) : ICoachingPlanCommandRepository
    {
        public async Task<ModelActionResult<CoachingPlanDetailsDao>> CreateCoachingPlanAsync(CoachingPlanCreateDao coachingPlanCreateDao)
        {
            try
            {
                var coachingPlanModel = coachingPlanCreateDao.ToModel();

                var coachModel = await _context.Users
                    .Include(u => u.Address)
                    .FirstOrDefaultAsync(u => u.Id == coachingPlanCreateDao.CoachId);
                if (coachModel is null)
                    return ModelActionResult<CoachingPlanDetailsDao>.Fail(GymFaultType.UserNotFound, "Coaching plan creation failed: coach not found.");

                if (coachModel.Role != Role.Coach)
                    return ModelActionResult<CoachingPlanDetailsDao>.Fail(GymFaultType.CoachingPlanCreationFailed, "Coaching plan creation failed: coach not found.");

                var clubModel = await _context.Clubs
                    .Include(u => u.Address)
                    .FirstOrDefaultAsync(u => u.Id == coachingPlanCreateDao.ClubId);
                if (clubModel is null)
                    return ModelActionResult<CoachingPlanDetailsDao>.Fail(GymFaultType.ClubNotFound, "Coaching plan creation failed: club not found.");

                coachingPlanModel.Coach = coachModel;
                coachingPlanModel.Club = clubModel;

                await _context.CoachingPlans.AddAsync(coachingPlanModel);
                var result = await _context.SaveChangesAsync();

                if (result <= 0)
                    return ModelActionResult<CoachingPlanDetailsDao>.Fail(GymFaultType.CoachingPlanCreationFailed, "Coaching plan creation failed: no rows affected.");

                return ModelActionResult<CoachingPlanDetailsDao>.Ok(coachingPlanModel.ToDetailsDao());
            }
            catch (DbUpdateException ex)
            {
                return ModelActionResult<CoachingPlanDetailsDao>.Fail(GymFaultType.DatabaseUnavailable, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
            catch (Exception ex)
            {
                return ModelActionResult<CoachingPlanDetailsDao>.Fail(GymFaultType.CoachingPlanCreationFailed, ex.Message);
            }
        }
    }
}

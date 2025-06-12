using GymManagement.Application.Interfaces.Repositories.MembershipPlans;
using GymManagement.Infrastructure.Repositories.Converters;
using GymManagement.Infrastructure.Repositories.DbContexts;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Repositories.MembershipPlans
{
    public class MembershipPlanCommandRepository(GymDbContext _context) : IMembershipPlanCommandRepository
    {
        public async Task<ModelActionResult<MembershipPlanDao>> CreateMembershipPlanAsync(MembershipPlanCreateDao membershipPlanCreateDao)
        {
            try
            {
                if (await _context.MembershipPlans.FirstOrDefaultAsync(mp => mp.IsValid && mp.MembershipPlanType == membershipPlanCreateDao.MembershipPlanType) != null)
                    return ModelActionResult<MembershipPlanDao>.Fail(GymFaultType.MembershipPlanCreationFailed, "Membership plan creation failed: membership plan with same MembershipPlanType already exists.");

                var membershipPlanModel = membershipPlanCreateDao.ToModel();

                await _context.MembershipPlans.AddAsync(membershipPlanModel);
                var result = await _context.SaveChangesAsync();

                if (result <= 0)
                    return ModelActionResult<MembershipPlanDao>.Fail(GymFaultType.MembershipPlanCreationFailed, "Membership plan creation failed: no rows affected.");

                return ModelActionResult<MembershipPlanDao>.Ok(membershipPlanModel.ToDao());
            }
            catch (DbUpdateException ex)
            {
                return ModelActionResult<MembershipPlanDao>.Fail(GymFaultType.DatabaseUnavailable, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
            catch (Exception ex)
            {
                return ModelActionResult<MembershipPlanDao>.Fail(GymFaultType.MembershipPlanCreationFailed, ex.Message);
            }
        }
    }
}

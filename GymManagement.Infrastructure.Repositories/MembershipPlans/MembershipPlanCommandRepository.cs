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

        public async Task<ModelActionResult> UpdateMembershipPlanAsync(MembershipPlanUpdateDao membershipPlanUpdateDao)
        {
            try
            {
                var membershipPlanModel = await _context.MembershipPlans
                    .FindAsync(membershipPlanUpdateDao.Id);

                if (membershipPlanModel == null)
                    return ModelActionResult.Fail(GymFaultType.MembershipPlanNotFound, "Membership plan update failed: membership plan not found.");

                if (await _context.MembershipPlans.AnyAsync(mp => mp.IsValid && mp.MembershipPlanType == membershipPlanUpdateDao.MembershipPlanType && mp.Id != membershipPlanUpdateDao.Id))
                    return ModelActionResult.Fail(GymFaultType.MembershipPlanUpdateFailed, "Membership plan update failed: membership plan with same MembershipPlanType already exists.");

                if (!string.IsNullOrWhiteSpace(membershipPlanUpdateDao.Description))
                    membershipPlanModel.Description = membershipPlanUpdateDao.Description;

                if (membershipPlanUpdateDao.MembershipPlanType.HasValue)
                    membershipPlanModel.MembershipPlanType = membershipPlanUpdateDao.MembershipPlanType.Value;

                if (membershipPlanUpdateDao.BasePrice.HasValue)
                    membershipPlanModel.BasePrice = membershipPlanUpdateDao.BasePrice.Value;

                if (membershipPlanUpdateDao.YearlyDiscount.HasValue)
                    membershipPlanModel.YearlyDiscount = membershipPlanUpdateDao.YearlyDiscount.Value;

                if (membershipPlanUpdateDao.RegistrationFees.HasValue)
                    membershipPlanModel.RegistrationFees = membershipPlanUpdateDao.RegistrationFees.Value;

                if (membershipPlanUpdateDao.IsValid.HasValue)
                    membershipPlanModel.IsValid = membershipPlanUpdateDao.IsValid.Value;

                _context.MembershipPlans.Update(membershipPlanModel);

                var result = await _context.SaveChangesAsync();
                if (result <= 0)
                    return ModelActionResult.Fail(GymFaultType.MembershipPlanUpdateFailed, "Membership plan update failed: no rows affected.");

                return ModelActionResult.Ok;
            }
            catch (DbUpdateException ex)
            {
                return ModelActionResult.Fail(GymFaultType.DatabaseUnavailable, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
            catch (Exception ex)
            {
                return ModelActionResult.Fail(GymFaultType.MembershipPlanUpdateFailed, ex.Message);
            }
        }
    }
}

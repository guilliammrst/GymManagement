using GymManagement.Application.Interfaces.Repositories.MembershipPlans;
using GymManagement.Infrastructure.Persistence.Models;

namespace GymManagement.Infrastructure.Repositories.Converters
{
    public static class MembershipPlanConverter
    {
        public static MembershipPlanModel ToModel(this MembershipPlanCreateDao membershipPlan)
        {
            return new MembershipPlanModel
            {
                Description = membershipPlan.Description,
                BasePrice = membershipPlan.BasePrice,
                RegistrationFees = membershipPlan.RegistrationFees,
                MembershipPlanType = membershipPlan.MembershipPlanType,
                YearlyDiscount = membershipPlan.YearlyDiscount,
                IsValid = membershipPlan.IsValid,
            };
        }

        public static MembershipPlanDao ToDao(this MembershipPlanModel membershipPlan)
        {
            return new MembershipPlanDao
            {
                Id = membershipPlan.Id,
                CreatedAt = membershipPlan.CreatedAt,
                UpdatedAt = membershipPlan.UpdatedAt,
                Description = membershipPlan.Description,
                BasePrice = membershipPlan.BasePrice,
                RegistrationFees = membershipPlan.RegistrationFees,
                MembershipPlanType = membershipPlan.MembershipPlanType,
                YearlyDiscount = membershipPlan.YearlyDiscount,
                IsValid = membershipPlan.IsValid
            };
        }

        public static List<MembershipPlanDao> ToDao(this IEnumerable<MembershipPlanModel> membershipPlans)
        {
            return membershipPlans.Select(ToDao).ToList();
        }

        public static MembershipPlanDetailsDao ToDetailsDao(this MembershipPlanModel membershipPlan)
        {
            return new MembershipPlanDetailsDao
            {
                Id = membershipPlan.Id,
                CreatedAt = membershipPlan.CreatedAt,
                UpdatedAt = membershipPlan.UpdatedAt,
                Description = membershipPlan.Description,
                BasePrice = membershipPlan.BasePrice,
                RegistrationFees = membershipPlan.RegistrationFees,
                MembershipPlanType = membershipPlan.MembershipPlanType,
                YearlyDiscount = membershipPlan.YearlyDiscount,
                IsValid = membershipPlan.IsValid,
                Memberships = membershipPlan.Memberships.ToDao()
            };
        }
    }
}

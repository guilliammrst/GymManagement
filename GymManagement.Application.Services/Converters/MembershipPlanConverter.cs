using GymManagement.Application.Interfaces.Repositories.MembershipPlans;
using GymManagement.Application.Interfaces.Services.MembershipPlans;
using GymManagement.Domain;

namespace GymManagement.Application.Services.Converters
{
    public static class MembershipPlanConverter
    {
        public static MembershipPlanUpdateDao ToUpdateDao(this MembershipPlan membershipPlan)
        {
            return new MembershipPlanUpdateDao
            {
                Id = membershipPlan.Id,
                Description = membershipPlan.Description,
                MembershipPlanType = membershipPlan.MembershipPlanType,
                RegistrationFees = membershipPlan.RegistrationFees,
                YearlyDiscount = membershipPlan.YearlyDiscount,
                BasePrice = membershipPlan.BasePrice,
                IsValid = membershipPlan.IsValid
            };
        }

        public static MembershipPlanCreateDao ToCreateDao(this MembershipPlan membershipPlan)
        {
            return new MembershipPlanCreateDao
            {
                Description = membershipPlan.Description,
                MembershipPlanType = membershipPlan.MembershipPlanType,
                RegistrationFees = membershipPlan.RegistrationFees,
                YearlyDiscount = membershipPlan.YearlyDiscount,
                BasePrice = membershipPlan.BasePrice,
                IsValid = membershipPlan.IsValid
            };
        }

        public static MembershipPlanDto ToDto(this MembershipPlanDao membershipPlan)
        {
            return new MembershipPlanDto
            {
                Id = membershipPlan.Id,
                CreatedAt = membershipPlan.CreatedAt,
                UpdatedAt = membershipPlan.UpdatedAt,
                Description = membershipPlan.Description,
                MembershipPlanType = membershipPlan.MembershipPlanType,
                RegistrationFees = membershipPlan.RegistrationFees,
                YearlyDiscount = membershipPlan.YearlyDiscount,
                BasePrice = membershipPlan.BasePrice,
                IsValid = membershipPlan.IsValid
            };
        }

        public static List<MembershipPlanDto> ToDto(this IEnumerable<MembershipPlanDao> membershipPlans)
        {
            return membershipPlans.Select(ToDto).ToList();
        }

        public static MembershipPlanDetailsDto ToDetailsDto(this MembershipPlanDetailsDao membershipPlan)
        {
            return new MembershipPlanDetailsDto
            {
                Id = membershipPlan.Id,
                CreatedAt = membershipPlan.CreatedAt,
                UpdatedAt = membershipPlan.UpdatedAt,
                Description = membershipPlan.Description,
                MembershipPlanType = membershipPlan.MembershipPlanType,
                RegistrationFees = membershipPlan.RegistrationFees,
                YearlyDiscount = membershipPlan.YearlyDiscount,
                BasePrice = membershipPlan.BasePrice,
                IsValid = membershipPlan.IsValid,
                Memberships = membershipPlan.Memberships.ToDto()
            };
        }
    }
}

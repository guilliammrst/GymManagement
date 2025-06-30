using GymManagement.Shared.Core.BaseObjects;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;

namespace GymManagement.Domain
{
    public class MembershipPlan : BaseObject
    {
        private MembershipPlan(string description, MembershipPlanType membershipPlanType, decimal basePrice, decimal yearlyDiscount, decimal registrationFees) : base (0, DateTime.UtcNow) 
        {
            Description = description;
            MembershipPlanType = membershipPlanType;
            BasePrice = basePrice;
            YearlyDiscount = yearlyDiscount;
            RegistrationFees = registrationFees;
            IsValid = true;
        }

        private MembershipPlan(int id, DateTime createdAt, DateTime? updatedAt, bool isValid, string description, MembershipPlanType membershipPlanType, decimal basePrice, decimal yearlyDiscount, decimal registrationFees) : base(id, createdAt, updatedAt)
        {
            Description = description;
            MembershipPlanType = membershipPlanType;
            BasePrice = basePrice;
            YearlyDiscount = yearlyDiscount;
            RegistrationFees = registrationFees;
            IsValid = isValid;
        }

        public string Description { get; private set; }
        public MembershipPlanType MembershipPlanType { get; private set; }
        public decimal BasePrice { get; private set; }
        public decimal YearlyDiscount { get; private set; }
        public decimal RegistrationFees { get; private set; }
        public bool IsValid { get; private set; }

        public static ModelActionResult<MembershipPlan> Create(string? description, MembershipPlanType? membershipPlanType, decimal? basePrice, decimal? yearlyDiscount, decimal? registrationFees)
        {
            if (string.IsNullOrWhiteSpace(description))
                return ModelActionResult<MembershipPlan>.Fail(GymFaultType.BadParameter, "Membership plan creation failed: field Description is required.");

            if (!membershipPlanType.HasValue)
                return ModelActionResult<MembershipPlan>.Fail(GymFaultType.BadParameter, "Membership plan creation failed: field MembershipPlanType is required.");

            if (!Enum.IsDefined((MembershipPlanType)membershipPlanType))
                return ModelActionResult<MembershipPlan>.Fail(GymFaultType.BadParameter, "Membership plan creation failed: field MembershipPlanType does not contain a valid value.");

            if (!basePrice.HasValue || basePrice <= 0)
                return ModelActionResult<MembershipPlan>.Fail(GymFaultType.BadParameter, "Membership plan creation failed: field BasePrice is required and must be greater than 0.");

            if (!yearlyDiscount.HasValue || yearlyDiscount < 0)
                return ModelActionResult<MembershipPlan>.Fail(GymFaultType.BadParameter, "Membership plan creation failed: field YearlyDiscount is required and must be greater than or equal to 0.");

            if (!registrationFees.HasValue || registrationFees < 0)
                return ModelActionResult<MembershipPlan>.Fail(GymFaultType.BadParameter, "Membership plan creation failed: field RegistrationFees is required and must be greater than or equal to 0.");

            return ModelActionResult<MembershipPlan>.Ok(new MembershipPlan(description, membershipPlanType.Value, basePrice.Value, yearlyDiscount.Value, registrationFees.Value));
        }

        public static ModelActionResult<MembershipPlan> Build(int id, DateTime createdAt, DateTime? updatedAt, bool isValid, string description, MembershipPlanType membershipPlanType, decimal basePrice, decimal yearlyDiscount, decimal registrationFees)
        {
            return ModelActionResult<MembershipPlan>.Ok(new MembershipPlan(id, createdAt, updatedAt, isValid, description, membershipPlanType, basePrice, yearlyDiscount, registrationFees));
        }

        public ModelActionResult Update(bool? isValid, string? description, MembershipPlanType? membershipPlanType, decimal? basePrice, decimal? yearlyDiscount, decimal? registrationFees)
        {
            if (isValid.HasValue)
                IsValid = isValid.Value;

            if (!string.IsNullOrWhiteSpace(description))
                Description = description;

            if (membershipPlanType.HasValue)
            {
                if (!Enum.IsDefined((MembershipPlanType)membershipPlanType))
                    return ModelActionResult.Fail(GymFaultType.BadParameter, "Membership plan creation failed: field MembershipPlanType does not contain a valid value.");

                MembershipPlanType = membershipPlanType.Value;
            }

            if (basePrice.HasValue)
            {
                if (basePrice <= 0)
                    return ModelActionResult.Fail(GymFaultType.BadParameter, "Membership plan creation failed: field BasePrice must be greater than 0.");
            
                BasePrice = basePrice.Value;
            }

            if (yearlyDiscount.HasValue)
            {
                if (yearlyDiscount < 0)
                    return ModelActionResult.Fail(GymFaultType.BadParameter, "Membership plan creation failed: field YearlyDiscount must be greater than or equal to 0.");

                YearlyDiscount = yearlyDiscount.Value;
            }

            if (registrationFees.HasValue)
            {
                if (registrationFees < 0)
                    return ModelActionResult.Fail(GymFaultType.BadParameter, "Membership plan creation failed: field RegistrationFees must be greater than or equal to 0.");
                RegistrationFees = registrationFees.Value;
            }

            return ModelActionResult.Ok;
        }
    }
}

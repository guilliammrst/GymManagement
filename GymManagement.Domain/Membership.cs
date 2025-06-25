using GymManagement.Shared.Core.BaseObjects;
using GymManagement.Shared.Core.Converters;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;

namespace GymManagement.Domain
{
    public class Membership : BaseObject
    {
        private Membership(DateTime startDate, DateTime endDate, bool isActive, bool renewWhenExpiry, PaymentDetail paymentDetail, MembershipPlan membershipPlan, Club homeClub) : base(0, DateTime.UtcNow)
        {
            StartDate = startDate;
            EndDate = endDate;
            IsActive = isActive;
            RenewWhenExpiry = renewWhenExpiry;
            IsExpired = DateTime.UtcNow > EndDate;
            PaymentDetail = paymentDetail;
            MembershipPlan = membershipPlan;
            HomeClub = homeClub;
        }

        private Membership(int id, DateTime createdAt, DateTime? updatedAt, DateTime startDate, DateTime endDate, bool isActive, bool renewWhenExpiry, bool isExpired, PaymentDetail? paymentDetail, MembershipPlan? membershipPlan, Club? homeClub) : base(id, createdAt, updatedAt)
        {
            StartDate = startDate;
            EndDate = endDate;
            IsActive = isActive;
            RenewWhenExpiry = renewWhenExpiry;
            IsExpired = isExpired;
            PaymentDetail = paymentDetail;
            MembershipPlan = membershipPlan;
            HomeClub = homeClub;
        }

        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public bool IsActive { get; private set; }
        public bool RenewWhenExpiry { get; }
        public bool IsExpired { get; }
        public PaymentDetail? PaymentDetail { get; }
        public MembershipPlan? MembershipPlan { get; }
        public Club? HomeClub { get; }

        public static ModelActionResult<Membership> Create(DateTime? startDate, MembershipPeriod? membershipPeriod, bool? renewWhenExpiry, MembershipPlan? membershipPlan, Club? homeClub)
        {
            if (!startDate.HasValue)
                return ModelActionResult<Membership>.Fail(GymFaultType.BadParameter, "Membership creation failed: field StartDate is required.");

            startDate = startDate.Value.ToUniversalTime();

            if (startDate.Value < DateTime.UtcNow)
                return ModelActionResult<Membership>.Fail(GymFaultType.BadParameter, "Membership creation failed: field StartDate cannot be in the past.");

            if (!membershipPeriod.HasValue)
                return ModelActionResult<Membership>.Fail(GymFaultType.BadParameter, "Membership creation failed: field MembershipPeriod is required.");

            if (!Enum.IsDefined(membershipPeriod.Value))
                return ModelActionResult<Membership>.Fail(GymFaultType.BadParameter, "Membership creation failed: field MembershipPeriod does not contain a valid value.");

            if (!renewWhenExpiry.HasValue)
                return ModelActionResult<Membership>.Fail(GymFaultType.BadParameter, "Membership creation failed: field RenewWhenExpiry is required.");

            var endDateResult = startDate.Value.ToEndDate(membershipPeriod.Value);
            if (!endDateResult.Success)
                return ModelActionResult<Membership>.Fail(endDateResult);

            var endDate = endDateResult.Results;

            if (membershipPlan is null)
                return ModelActionResult<Membership>.Fail(GymFaultType.BadParameter, "Membership creation failed: field MembershipPlan is required.");

            if (homeClub is null)
                return ModelActionResult<Membership>.Fail(GymFaultType.BadParameter, "Membership creation failed: field HomeClub is required.");

            var paymentDetailResult = PaymentDetail.Create(membershipPeriod.Value, membershipPlan.BasePrice, membershipPlan.YearlyDiscount, membershipPlan.RegistrationFees);
            if (!paymentDetailResult.Success)
                return ModelActionResult<Membership>.Fail(paymentDetailResult);

            return ModelActionResult<Membership>.Ok(new Membership(startDate.Value, endDate, false, renewWhenExpiry.Value, paymentDetailResult.Results, membershipPlan, homeClub));
        }

        public static ModelActionResult<Membership> Build(int id, DateTime createdAt, DateTime? updatedAt, DateTime startDate, DateTime endDate, bool isActive, bool renewWhenExpiry, bool isExpired, PaymentDetail? paymentDetail = null, MembershipPlan? membershipPlan = null, Club? homeClub = null)
        {
            return ModelActionResult<Membership>.Ok(new Membership(id, createdAt, updatedAt, startDate, endDate, isActive, renewWhenExpiry, isExpired, paymentDetail, membershipPlan, homeClub));
        }

        public ModelActionResult Pay(PaymentMethod? paymentMethod)
        {
            if (IsExpired)
                return ModelActionResult.Fail(GymFaultType.MembershipExpired, "Membership payment failed: membership is expired.");

            if (PaymentDetail == null)
                return ModelActionResult.Fail(GymFaultType.MembershipPaymentDetailNotFound, "Membership payment failed: payment detail not found.");

            var paymentDetailResult = PaymentDetail.Pay(paymentMethod);
            if (!paymentDetailResult.Success)
                return ModelActionResult.Fail(paymentDetailResult);

            IsActive = true;
            return ModelActionResult.Ok;
        }
    }
}

using GymManagement.Shared.Core.BaseObjects;
using GymManagement.Shared.Core.Converters;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;

namespace GymManagement.Domain
{
    public class Coaching : BaseObject
    {
        private Coaching(DateTime startDate, DateTime endDate, bool isActive, bool renewWhenExpiry, WeekDays weekDay, int hour, PaymentDetail paymentDetail, CoachingPlan coachingPlan, User user) : base(0, DateTime.UtcNow)
        {
            StartDate = startDate;
            EndDate = endDate;
            IsActive = isActive;
            RenewWhenExpiry = renewWhenExpiry;
            WeekDay = weekDay;
            Hour = hour;
            PaymentDetail = paymentDetail;
            CoachingPlan = coachingPlan;
            Member = user;
        }

        private Coaching(int id, DateTime createdAt, DateTime? updatedAt, DateTime startDate, DateTime endDate, bool isActive, bool renewWhenExpiry, WeekDays weekDay, int hour, PaymentDetail? paymentDetail, CoachingPlan? coachingPlan, User? user) : base(id, createdAt, updatedAt)
        {
            StartDate = startDate;
            EndDate = endDate;
            IsActive = isActive;
            RenewWhenExpiry = renewWhenExpiry;
            WeekDay = weekDay;
            Hour = hour;
            PaymentDetail = paymentDetail;
            CoachingPlan = coachingPlan;
            Member = user;
        }

        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public bool IsActive { get; private set; }
        public bool RenewWhenExpiry { get; }
        public WeekDays WeekDay { get; }
        public int Hour { get; }
        public PaymentDetail? PaymentDetail { get; }
        public CoachingPlan? CoachingPlan { get; }
        public User? Member { get; set; }

        public static ModelActionResult<Coaching> Create(DateTime? startDate, bool? renewWhenExpiry, WeekDays? weekDay, int? hour, CoachingPlan? coachingPlan, User? user)
        {
            if (!startDate.HasValue)
                return ModelActionResult<Coaching>.Fail(GymFaultType.BadParameter, "Coaching creation failed: field StartDate is required.");

            startDate = startDate.Value.ToUniversalTime();

            if (startDate.Value < DateTime.UtcNow)
                return ModelActionResult<Coaching>.Fail(GymFaultType.BadParameter, "Coaching creation failed: field StartDate cannot be in the past.");

            if (!weekDay.HasValue)
                return ModelActionResult<Coaching>.Fail(GymFaultType.BadParameter, "Coaching creation failed: field WeekDay is required.");

            if (!Enum.IsDefined(weekDay.Value))
                return ModelActionResult<Coaching>.Fail(GymFaultType.BadParameter, "Coaching creation failed: field WeekDay does not contain a valid value.");

            if (!renewWhenExpiry.HasValue)
                return ModelActionResult<Coaching>.Fail(GymFaultType.BadParameter, "Coaching creation failed: field RenewWhenExpiry is required.");

            var endDateResult = startDate.Value.ToEndDate(MembershipPeriod.FourWeeks);
            if (!endDateResult.Success)
                return ModelActionResult<Coaching>.Fail(endDateResult);

            var endDate = endDateResult.Results;

            if (!hour.HasValue)
                return ModelActionResult<Coaching>.Fail(GymFaultType.BadParameter, "Coaching creation failed: field Hour is required.");

            if (hour < 8 || hour > 20)
                return ModelActionResult<Coaching>.Fail(GymFaultType.BadParameter, "Coaching creation failed: field Hour must be between 8 and 20.");

            if (coachingPlan == null)
                return ModelActionResult<Coaching>.Fail(GymFaultType.BadParameter, "Coaching creation failed: field CoachingPlan is required.");

            if (!coachingPlan.IsValid)
                return ModelActionResult<Coaching>.Fail(GymFaultType.BadParameter, "Coaching creation failed: field CoachingPlan is not valid.");

            var paymentDetailResult = PaymentDetail.Create(coachingPlan.Price);
            if (!paymentDetailResult.Success)
                return ModelActionResult<Coaching>.Fail(paymentDetailResult);

            if (user == null)
                return ModelActionResult<Coaching>.Fail(GymFaultType.BadParameter, "Coaching creation failed: field User is required.");

            if (user.Memberships == null || !user.Memberships.Any())
                return ModelActionResult<Coaching>.Fail(GymFaultType.BadParameter, "Coaching creation failed: user does not have any memberships.");

            if (user.Memberships.All(m => !m.IsActive))
                return ModelActionResult<Coaching>.Fail(GymFaultType.BadParameter, "Coaching creation failed: user does not have any active memberships.");

            return ModelActionResult<Coaching>.Ok(new Coaching(startDate.Value, endDate, false, renewWhenExpiry.Value, weekDay.Value, hour.Value, paymentDetailResult.Results, coachingPlan, user));
        }

        public static ModelActionResult<Coaching> Build(int id, DateTime createdAt, DateTime? updatedAt, DateTime startDate, DateTime endDate, bool isActive, bool renewWhenExpiry, WeekDays weekDay, int hour, PaymentDetail? paymentDetail = null, CoachingPlan? coachingPlan = null, User? user = null)
        {
            return ModelActionResult<Coaching>.Ok(new Coaching(id, createdAt, updatedAt, startDate, endDate, isActive, renewWhenExpiry, weekDay, hour, paymentDetail, coachingPlan, user));
        }

        public ModelActionResult Pay(PaymentMethod? paymentMethod)
        {
            if (PaymentDetail == null)
                return ModelActionResult.Fail(GymFaultType.CoachingPaymentDetailNotFound, "Coaching payment failed: payment detail not found.");

            var paymentDetailResult = PaymentDetail.Pay(paymentMethod);
            if (!paymentDetailResult.Success)
                return ModelActionResult.Fail(paymentDetailResult);

            IsActive = true;
            return ModelActionResult.Ok;
        }
    }
}

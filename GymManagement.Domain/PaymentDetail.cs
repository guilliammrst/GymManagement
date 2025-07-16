using GymManagement.Shared.Core.BaseObjects;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;

namespace GymManagement.Domain
{
    public class PaymentDetail : BaseObject
    {
        private PaymentDetail(decimal amount) : base(0, DateTime.UtcNow)
        {
            Amount = amount;
            PaymentMethod = PaymentMethod.None;
            PaymentStatus = PaymentStatus.Pending;
        }

        private PaymentDetail(int id, DateTime createdAt, DateTime? updatedAt, decimal amount, PaymentMethod paymentMethod, PaymentStatus paymentStatus, DateTime? paymentDate) : base(id, createdAt, updatedAt)
        {
            Amount = amount;
            PaymentMethod = paymentMethod;
            PaymentStatus = paymentStatus;
            PaymentDate = paymentDate;
        }

        public decimal Amount { get; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime? PaymentDate { get; set; }

        public static ModelActionResult<PaymentDetail> Create(MembershipPeriod? membershipPeriod, decimal? basePrice, decimal? yearlyDiscount, decimal? registrationFees)
        {
            if (!membershipPeriod.HasValue)
                return ModelActionResult<PaymentDetail>.Fail(GymFaultType.BadParameter, "Payment detail creation failed: field MembershipPeriod is required.");

            if (!Enum.IsDefined(membershipPeriod.Value))
                return ModelActionResult<PaymentDetail>.Fail(GymFaultType.BadParameter, "Payment detail creation failed: field MembershipPeriod does not contain a valid value.");

            if (!basePrice.HasValue || basePrice <= 0)
                return ModelActionResult<PaymentDetail>.Fail(GymFaultType.BadParameter, "Payment detail creation failed: field Amount must be greater than zero.");

            if (!yearlyDiscount.HasValue || yearlyDiscount < 0)
                return ModelActionResult<PaymentDetail>.Fail(GymFaultType.BadParameter, "Payment detail creation failed: field YearlyDiscount must be zero or greater.");

            if (!registrationFees.HasValue || registrationFees < 0)
                return ModelActionResult<PaymentDetail>.Fail(GymFaultType.BadParameter, "Payment detail creation failed: field RegistrationFees must be zero or greater.");

            decimal amount = CalculateAmount(membershipPeriod.Value, basePrice.Value, yearlyDiscount.Value, registrationFees.Value);

            return ModelActionResult<PaymentDetail>.Ok(new PaymentDetail(amount));
        }

        public static ModelActionResult<PaymentDetail> Create(decimal? price)
        {
            if (!price.HasValue || price <= 0)
                return ModelActionResult<PaymentDetail>.Fail(GymFaultType.BadParameter, "Payment detail creation failed: field Amount must be greater than zero.");
            
            return ModelActionResult<PaymentDetail>.Ok(new PaymentDetail(price.Value));
        }

        public static ModelActionResult<PaymentDetail> Build(int id, DateTime createdAt, DateTime? updatedAt, decimal amount, PaymentMethod paymentMethod, PaymentStatus paymentStatus, DateTime? paymentDate)
        {
            return ModelActionResult<PaymentDetail>.Ok(new PaymentDetail(id, createdAt, updatedAt, amount, paymentMethod, paymentStatus, paymentDate));
        }

        public ModelActionResult Pay(PaymentMethod? paymentMethod)
        {
            if (PaymentStatus == PaymentStatus.Paid)
                return ModelActionResult.Fail(GymFaultType.MembershipAlreadyPaid, "Payment detail payment failed: membership already paid.");

            if (!paymentMethod.HasValue)
                return ModelActionResult.Fail(GymFaultType.BadParameter, "Payment detail payment failed: field PaymentMethod is required.");

            if (!Enum.IsDefined(paymentMethod.Value))
                return ModelActionResult.Fail(GymFaultType.BadParameter, "Payment detail payment failed: field PaymentMethod does not contain a valid value.");

            if (paymentMethod == PaymentMethod.None)
                return ModelActionResult.Fail(GymFaultType.BadParameter, "Payment detail payment failed: field PaymentMethod is None.");

            PaymentMethod = paymentMethod.Value;
            PaymentStatus = PaymentStatus.Paid;
            PaymentDate = DateTime.UtcNow;
            return ModelActionResult.Ok;
        }

        private static decimal CalculateAmount(MembershipPeriod membershipPeriod, decimal basePrice, decimal yearlyDiscount, decimal registrationFees)
        {
            decimal amount = basePrice;
            switch (membershipPeriod)
            {
                case MembershipPeriod.FourWeeks:
                    amount *= 1; // One period of 4 weeks
                    break;
                case MembershipPeriod.OneYear:
                    const int WeeksInYear = 13; // 13 * 4 weeks = 52 weeks
                    amount *= WeeksInYear;
                    if (yearlyDiscount > 0)
                        amount *= (100 - yearlyDiscount) / 100;
                    break;
            }

            amount += registrationFees;
            return amount > 0 ? amount : 0;
        }
    }
}

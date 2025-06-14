using GymManagement.Shared.Core.Enums;

namespace GymManagement.Application.Interfaces.Repositories.Users
{
    public class UserPaymentDao
    {
        public required int MembershipId { get; set; }
        public required bool IsActive { get; set; }
        public required DateTime PaymentDate { get; set; }
        public required PaymentMethod PaymentMethod { get; set; }
        public required PaymentStatus PaymentStatus { get; set; }
    }
}

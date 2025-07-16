using GymManagement.Shared.Core.Enums;

namespace GymManagement.Application.Interfaces.Services.Users
{
    public class UserPaymentDto
    {
        public int UserId { get; set; }
        public int EntityId { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
    }
}

using GymManagement.Shared.Core.Enums;

namespace GymManagement.Application.Interfaces.Controllers.DTOs
{
    public class PaymentDto
    {
        public PaymentMethod? PaymentMethod { get; set; }
    }
}

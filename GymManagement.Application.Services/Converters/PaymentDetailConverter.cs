using GymManagement.Application.Interfaces.Repositories.PaymentDetails;
using GymManagement.Application.Interfaces.Services.PaymentDetails;

namespace GymManagement.Application.Services.Converters
{
    public static class PaymentDetailConverter
    {
        public static PaymentDetailDto ToDto(this PaymentDetailDao paymentDetail)
        {
            return new PaymentDetailDto
            {
                Id = paymentDetail.Id,
                CreatedAt = paymentDetail.CreatedAt,
                UpdatedAt = paymentDetail.UpdatedAt,
                PaymentDate = paymentDetail.PaymentDate,
                Amount = paymentDetail.Amount,
                PaymentMethod = paymentDetail.PaymentMethod,
                PaymentStatus = paymentDetail.PaymentStatus
            };
        }
    }
}

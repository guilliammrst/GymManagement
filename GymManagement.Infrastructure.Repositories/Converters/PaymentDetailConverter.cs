using GymManagement.Application.Interfaces.Repositories.PaymentDetails;
using GymManagement.Infrastructure.Persistence.Models;

namespace GymManagement.Infrastructure.Repositories.Converters
{
    public static class PaymentDetailConverter
    {
        public static PaymentDetailDao ToDao(this PaymentDetailModel paymentDetail)
        {
            return new PaymentDetailDao
            {
                Id = paymentDetail.Id,
                CreatedAt = paymentDetail.CreatedAt,
                UpdatedAt = paymentDetail.UpdatedAt,
                Amount = paymentDetail.Amount,
                PaymentDate = paymentDetail.PaymentDate,
                PaymentMethod = paymentDetail.PaymentMethod,
                PaymentStatus = paymentDetail.PaymentStatus
            };
        }
    }
}

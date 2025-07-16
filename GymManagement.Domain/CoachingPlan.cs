using GymManagement.Shared.Core.BaseObjects;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;

namespace GymManagement.Domain
{
    public class CoachingPlan : BaseObject
    {
        private CoachingPlan(string description, decimal price, int coachId, int clubId) : base(0, DateTime.UtcNow)
        {
            Description = description;
            Price = price;
            IsValid = true;
            CoachId = coachId;
            ClubId = clubId;
        }

        private CoachingPlan(int id, DateTime createdAt, DateTime? updatedAt, bool isValid, string description, decimal price, int? coachId, int? clubId) : base(id, createdAt, updatedAt)
        {
            Description = description;
            Price = price;
            IsValid = isValid;
            CoachId = coachId;
            ClubId = clubId;
        }

        public string Description { get; }
        public decimal Price { get; }
        public bool IsValid { get; }
        public int? CoachId { get; }
        public int? ClubId { get; }

        public static ModelActionResult<CoachingPlan> Create(string? description, decimal? price, int? coachId, int? clubId)
        {
            if (string.IsNullOrWhiteSpace(description))
                return ModelActionResult<CoachingPlan>.Fail(GymFaultType.BadParameter, "Coaching plan creation failed: field Description cannot be empty.");

            if (!price.HasValue || price <= 0)
                return ModelActionResult<CoachingPlan>.Fail(GymFaultType.BadParameter, "Coaching plan creation failed: field Price must be greater than zero.");

            if (!coachId.HasValue || coachId <= 0)
                return ModelActionResult<CoachingPlan>.Fail(GymFaultType.BadParameter, "Coaching plan creation failed: field CoachId must be greater than zero.");

            if (!clubId.HasValue || clubId <= 0)
                return ModelActionResult<CoachingPlan>.Fail(GymFaultType.BadParameter, "Coaching plan creation failed: field ClubId must be greater than zero.");

            return ModelActionResult<CoachingPlan>.Ok(new CoachingPlan(description, price.Value, coachId.Value, clubId.Value));
        }

        public static ModelActionResult<CoachingPlan> Build(int id, DateTime createdAt, DateTime? updatedAt, bool isValid, string description, decimal price, int? coachId = null, int? clubId = null)
        {
            return ModelActionResult<CoachingPlan>.Ok(new CoachingPlan(id, createdAt, updatedAt, isValid, description, price, coachId, clubId));
        }
    }
}

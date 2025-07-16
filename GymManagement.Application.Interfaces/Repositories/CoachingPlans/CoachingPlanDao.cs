using GymManagement.Shared.Core.BaseObjects;

namespace GymManagement.Application.Interfaces.Repositories.CoachingPlans
{
    public class CoachingPlanDao : BaseDao
    {
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsValid { get; set; }
    }
}

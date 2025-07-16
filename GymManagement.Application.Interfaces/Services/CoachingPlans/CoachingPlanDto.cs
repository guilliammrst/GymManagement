using GymManagement.Shared.Core.BaseObjects;

namespace GymManagement.Application.Interfaces.Services.CoachingPlans
{
    public class CoachingPlanDto : BaseDto
    {
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsValid { get; set; }
    }
}

using GymManagement.Application.Interfaces.Services.Clubs;
using GymManagement.Application.Interfaces.Services.Coachings;
using GymManagement.Application.Interfaces.Services.Users;
using GymManagement.Shared.Core.BaseObjects;

namespace GymManagement.Application.Interfaces.Services.CoachingPlans
{
    public class CoachingPlanDetailsDto : BaseDto
    {
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsValid { get; set; }
        public UserDto Coach { get; set; } = new();
        public ClubDto Club { get; set; } = new();
        public ICollection<CoachingDto> Coachings { get; set; } = [];
    }
}

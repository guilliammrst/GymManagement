using GymManagement.Application.Interfaces.Repositories.Clubs;
using GymManagement.Application.Interfaces.Repositories.Coachings;
using GymManagement.Application.Interfaces.Repositories.Users;
using GymManagement.Shared.Core.BaseObjects;

namespace GymManagement.Application.Interfaces.Repositories.CoachingPlans
{
    public class CoachingPlanDetailsDao : BaseDao
    {
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsValid { get; set; }
        public UserDao Coach { get; set; } = new ();
        public ClubDao Club { get; set; } = new ();
        public ICollection<CoachingDao> Coachings { get; set; } = [];
    }
}

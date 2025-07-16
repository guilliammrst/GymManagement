namespace GymManagement.Application.Interfaces.Repositories.CoachingPlans
{
    public class CoachingPlanCreateDao
    {
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required bool IsValid { get; set; }
        public required int CoachId { get; set; }
        public required int ClubId { get; set; }
    }
}
